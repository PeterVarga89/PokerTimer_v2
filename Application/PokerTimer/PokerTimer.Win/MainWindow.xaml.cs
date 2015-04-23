using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;
using PokerTimer.Win.Dialogs;

namespace PokerTimer.Win
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        # region Properities

        private TournamentSettingsDlg Settings { get; set; }

        private Tournament _tournament;
        public Tournament Tournament
        {
            get { return _tournament; }
            set { _tournament = value; _tournament.LoadTournamentDetails(); Refresh(); }
        }

        private Structure _structure;
        public Structure Structure
        {
            get { return _structure; }
            set { _structure = value; _structure.LoadDetails(eConnectionString.Online); SetLevelDetails(); }
        }

        private bool _isShadeVisible;
        public bool IsShadeVisible
        {
            get
            {
                return _isShadeVisible;
            }
            set
            {
                _isShadeVisible = value;
                Refresh();
            }
        }

        private bool _isAuto;
        public bool IsAuto
        {
            get
            {
                return _isAuto;
            }
            set
            {
                _isAuto = value;
            }
        }

        # region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            IsShadeVisible,
            Tournament,
            CurrentBlindInfo,
            CurrentAnteInfo,
            NextBlindInfo,
            LevelInfo,

            Entries,
            ReBuyCount,
            AddOnCount,

            AvgStack,

            RebuyControlTitle,
            AddonControlTitle,

            PrizePoolDisplayName,
            LeagueMoneyDisplayName,
            MoneyPoolDisplayName,

            RebuyVisibility,
            AddonVisibility,

            PlacesInfo
        }

        public void Refresh()
        {
            PropertyChange(Property.IsShadeVisible);
            PropertyChange(Property.Tournament);

            PropertyChange(Property.PlacesInfo);

            PropertyChange(Property.CurrentBlindInfo);
            PropertyChange(Property.CurrentAnteInfo);
            PropertyChange(Property.NextBlindInfo);
            PropertyChange(Property.LevelInfo);

            PropertyChange(Property.Entries);
            PropertyChange(Property.ReBuyCount);
            PropertyChange(Property.AddOnCount);

            PropertyChange(Property.AvgStack);

            PropertyChange(Property.RebuyControlTitle);
            PropertyChange(Property.AddonControlTitle);

            PropertyChange(Property.PrizePoolDisplayName);
            PropertyChange(Property.LeagueMoneyDisplayName);
            PropertyChange(Property.MoneyPoolDisplayName);

            PropertyChange(Property.RebuyVisibility);
            PropertyChange(Property.AddonVisibility);
        }

        # endregion

        # endregion

        public MainWindow()
        {
            App.ParentWindow = this;
            DataContext = this;
            InitializeComponent();

            Settings = new TournamentSettingsDlg();

            IsShadeVisible = false;
            SetRandomBackground();

            timerCtrl.LevelCompleted += timerCtrl_LevelCompleted;

            PlayerList = new ObservableCollection<TournamentResult>();
            PlayerList.CollectionChanged += PlayerList_CollectionChanged;

            CurrentLevel = 1;

            this.KeyUp += MainWindow_KeyUp;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IsAuto = true;
            ManualPlaces = new List<int>();
            ManualPlaces = GetPlaces().ToList();

            var dlg = new Dialogs.SelectTournamentDlg();
            dlg.ShowDialog();
        }

        private void MainWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Space)
            {
                timerCtrl.ToggleStartStop();
            }

            if (e.Key == System.Windows.Input.Key.F1)
            {
                var buyInDlg = new Dialogs.RegisterUserDlg();
                buyInDlg.ShowDialog();
            }

            if (e.Key == System.Windows.Input.Key.F2)
            {
                if (Settings.IsNull())
                    Settings = new Dialogs.TournamentSettingsDlg();

                Settings.Refresh();
                Settings.ShowDialog();
            }

            if (e.Key == System.Windows.Input.Key.F3)
            {
                var buyInDlg = new Dialogs.PlayerListDlg();
                buyInDlg.ShowDialog();
            }
        }

        private void SetRandomBackground()
        {
            Random random = new Random();
            var num = random.Next(1, 9);

            var name = string.Format("bg_00{0}", num);

            imgBackground.Source = (ImageSource)FindResource(name);
        }

        # region Level & Structure

        private int CurrentLevel;

        private void timerCtrl_LevelCompleted(object sender, RoutedEventArgs e)
        {
            SetLevelDetails();
        }

        private void SetLevelDetails()
        {
            timerCtrl.LevelTimeMinutes = GetLevelTime();
            SetRandomBackground();
            Refresh();
        }

        private int GetLevelTime()
        {
            var structure = this.Structure.Details[Structure.Details.Count >= CurrentLevel - 1 ? CurrentLevel - 1 : Structure.Details.Count - 1];
            return structure.Time;
        }

        public string LevelInfo { get { return string.Format("LEVEL {0:00}", CurrentLevel); } }

        public string CurrentBlindInfo { get { return string.Format("{0}/{1}", GetCurrentStructure().SmallBlind, GetCurrentStructure().BigBlind); } }

        public string NextBlindInfo { get { return string.Format("{0}/{1} (Ante {2})", GetNextCurrentStructure().SmallBlind, GetNextCurrentStructure().BigBlind, GetNextCurrentStructure().Ante); } }

        public string CurrentAnteInfo { get { return GetCurrentStructure().Ante.ToString(); } }

        private StructureDetail GetCurrentStructure()
        {
            if (Structure.IsNull())
                return new StructureDetail();

            return Structure.Details[Structure.Details.Count >= CurrentLevel - 1 ? CurrentLevel - 1 : Structure.Details.Count - 1];
        }

        private StructureDetail GetNextCurrentStructure()
        {
            if (Structure.IsNull())
                return new StructureDetail();

            return Structure.Details[Structure.Details.Count >= CurrentLevel + 1 ? CurrentLevel + 1 : Structure.Details.Count];
        }

        # endregion

        # region Players

        public ObservableCollection<TournamentResult> PlayerList { get; set; }

        private void PlayerList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        public string Entries { get { return string.Format("{0}/{1}", PlayerCount, PlayerCountActive); } private set { } }

        public int PlayerCount { get { return PlayerList.IsNotNull() ? PlayerList.Count + PlayerList.Where(p => p.DateReEntry.HasValue).Count() : 0; } private set { } }

        public int PlayerCountActive { get { return PlayerList.IsNotNull() ? PlayerList.Where(p => !p.DateDeleted.HasValue).Count() : 0; } private set { } }

        public int ReEntryCount { get { return PlayerList.IsNotNull() ? PlayerList.Where(p => p.DateReEntry.HasValue).Count() : 0; } private set { } }

        public int ReBuyCount { get { return PlayerList.IsNotNull() ? PlayerList.Sum(p => p.ReBuyCount) : 0; } private set { } }

        public int AddOnCount { get { return PlayerList.IsNotNull() ? PlayerList.Sum(p => p.AddOnCount) : 0; } private set { } }

        public List<int> ManualPlaces { get; set; }

        # endregion

        # region Money

        public int BonusStackCount { get { return PlayerList.IsNotNull() ? PlayerList.Sum(p => p.BonusStackTotal) : 0; } private set { } }

        public int ChipsTotal
        {
            get
            {
                if (PlayerCount == 0)
                    return 0;

                var buyIns = PlayerCount * Tournament.TournamentDetail.BuyInStack;
                var reBuys = ReBuyCount * Tournament.TournamentDetail.ReBuyStack;
                var addOns = AddOnCount * Tournament.TournamentDetail.AddOnStack;

                return buyIns + reBuys + addOns + BonusStackCount;
            }
            private set
            {
            }
        }

        public int AvgStack { get { return PlayerCount != 0 ? ChipsTotal / PlayerCountActive : 0; } private set { } }

        public int TotalBank { get { return PlayerCount * Tournament.TournamentDetail.BuyInPrize; } private set { } }

        public Double MoneyPool { get { return PlayerList.IsNotNull() && Tournament.IsNotNull() ? PlayerList.Count * Tournament.TournamentDetail.BuyInPrize + ReBuyCount * Tournament.TournamentDetail.ReBuyPrize + AddOnCount * Tournament.TournamentDetail.AddOnPrize : 0; } private set { } }

        public Double LeagueMoney { get { return Tournament.IsNotNull() && Tournament.TournamentDetail.IsLeague ? (MoneyPool * 0.10).GetRounded() : 0; } private set { } }

        public Double RakeMoney { get { return Tournament.IsNotNull() && Tournament.TournamentDetail.IsLeague ? (MoneyPool * 0.15).GetRounded() : 0; } private set { } }

        public Double PrizePool { get { return (MoneyPool - RakeMoney).GetRounded(); } private set { } }

        public Double Dotation { get { return Tournament.IsNotNull() ? Math.Abs(Tournament.TournamentDetail.GTD.HasValue && Tournament.TournamentDetail.GTD.Value > PrizePool ? Tournament.TournamentDetail.GTD.Value - PrizePool : 0) : 0; } private set { } }

        public Double WinMoney { get { return (PrizePool + Dotation - LeagueMoney).GetRounded(); } private set { } }

        public string PrizePoolDisplayName { get { return PrizePool + "€"; } private set { } }

        public string LeagueMoneyDisplayName { get { return LeagueMoney + "€"; } private set { } }

        public string MoneyPoolDisplayName { get { return MoneyPool + "€"; } private set { } }

        public Double[] GetPercents()
        {
            var enumlist = Enum.GetValues(typeof(ePositionSeqs)).Cast<ePositionSeqs>().Select(p => (int)p).ToList();
            var selectedEnum = (ePositionSeqs)enumlist.Where(p => p <= PlayerList.Count).Max();
            var positionEnum = (ePositions)Enum.Parse(typeof(ePositions), selectedEnum.ToString());
            var percents = positionEnum.GetEnumDescription().Split('_').Select(p => Double.Parse(p)).ToArray();

            return percents;
        }

        public int[] GetPlaces()
        {
            int[] paymentList = new int[20];
            var percents = GetPercents();

            paymentList[0] = (int)GetNumValViaPercents(WinMoney, percents[0]);
            paymentList[1] = (int)GetNumValViaPercents(WinMoney, percents[1]);
            paymentList[2] = (int)GetNumValViaPercents(WinMoney, percents[2]);
            paymentList[3] = (int)GetNumValViaPercents(WinMoney, percents[3]);
            paymentList[4] = (int)GetNumValViaPercents(WinMoney, percents[4]);
            paymentList[5] = (int)GetNumValViaPercents(WinMoney, percents[5]);
            paymentList[6] = (int)GetNumValViaPercents(WinMoney, percents[6]);
            paymentList[7] = (int)GetNumValViaPercents(WinMoney, percents[7]);
            paymentList[8] = (int)GetNumValViaPercents(WinMoney, percents[8]);
            paymentList[9] = (int)GetNumValViaPercents(WinMoney, percents[9]);
            paymentList[10] = (int)GetNumValViaPercents(WinMoney, percents[10]);
            paymentList[11] = (int)GetNumValViaPercents(WinMoney, percents[11]);
            paymentList[12] = (int)GetNumValViaPercents(WinMoney, percents[12]);
            paymentList[13] = (int)GetNumValViaPercents(WinMoney, percents[13]);
            paymentList[14] = (int)GetNumValViaPercents(WinMoney, percents[14]);
            paymentList[15] = (int)GetNumValViaPercents(WinMoney, percents[15]);
            paymentList[16] = (int)GetNumValViaPercents(WinMoney, percents[16]);
            paymentList[17] = (int)GetNumValViaPercents(WinMoney, percents[17]);
            paymentList[18] = (int)GetNumValViaPercents(WinMoney, percents[18]);
            paymentList[19] = (int)GetNumValViaPercents(WinMoney, percents[19]);

            if (WinMoney - paymentList.Sum(p => p) > 0)
            {
                paymentList[0] = paymentList[0] + ((int)WinMoney - paymentList.Sum(p => p));
            }

            return paymentList;
        }

        public static Double GetNumValViaPercents(Double origin, Double percent)
        {
            if (percent == 0 || origin == 0)
                return 0;

            var val = Math.Round(origin * ((Double)percent / (Double)100), 0);

            return val - (val % 5);
        }

        public string PlacesInfo
        {
            get
            {
                if (Settings.IsNotNull())
                {
                    Settings.Refresh();
                    return string.Format("1. {0}€ </br>2. {1}€", Settings.Place01, Settings.Place02);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        # endregion

        # region Info Control Titles

        public string RebuyControlTitle { get { return Tournament.IsNotNull() ? Tournament.GameTypeEnum.In(eGameType.DoubleChance, eGameType.TripleChance) ? "2ND CHANGE" : "REBUY" : string.Empty; } private set { } }

        public string AddonControlTitle { get { return Tournament.IsNotNull() ? Tournament.GameTypeEnum.In(eGameType.TripleChance) ? "3RD CHANGE" : "ADD-ON" : string.Empty; } private set { } }

        public Visibility RebuyVisibility { get { return (Tournament.IsNotNull() && Tournament.GameTypeEnum.In(eGameType.DoubleChance, eGameType.DoubleTrouble, eGameType.FreeRoll, eGameType.RebuyLimited, eGameType.RebuyUnlimited)).ToVisibility(); } }

        public Visibility AddonVisibility { get { return (Tournament.IsNotNull() && Tournament.GameTypeEnum.In(eGameType.FreeRoll, eGameType.RebuyLimited, eGameType.RebuyUnlimited)).ToVisibility(); } }

        # endregion
    }
}