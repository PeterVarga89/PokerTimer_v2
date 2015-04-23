using System;
using System.ComponentModel;
using System.Windows;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.Win.Dialogs
{
    public partial class TournamentSettingsDlg : Window, INotifyPropertyChanged
    {
        # region Properities

        # region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            IsAuto,
            IsManual,

            Place01,
            Place02,
            Place03,
            Place04,
            Place05,
            Place06,
            Place07,
            Place08,
            Place09,
            Place10,

            Place11,
            Place12,
            Place13,
            Place14,
            Place15,
            Place16,
            Place17,
            Place18,
            Place19,
            Place20,

            Tournament,
            Detail,

            Rake,
            Dotation,
            Moneypool,
            League,
            PrizePool,
            WinnerMoney
        }

        public void Refresh()
        {
            PropertyChange(Property.Rake);
            PropertyChange(Property.Dotation);
            PropertyChange(Property.Moneypool);
            PropertyChange(Property.League);
            PropertyChange(Property.PrizePool);
            PropertyChange(Property.WinnerMoney);

            PropertyChange(Property.Tournament);
            PropertyChange(Property.Detail);

            PropertyChange(Property.IsAuto);
            PropertyChange(Property.IsManual);

            PropertyChange(Property.Place01);
            PropertyChange(Property.Place02);
            PropertyChange(Property.Place03);
            PropertyChange(Property.Place04);
            PropertyChange(Property.Place05);
            PropertyChange(Property.Place06);
            PropertyChange(Property.Place07);
            PropertyChange(Property.Place08);
            PropertyChange(Property.Place09);
            PropertyChange(Property.Place10);
            PropertyChange(Property.Place11);
            PropertyChange(Property.Place12);
            PropertyChange(Property.Place13);
            PropertyChange(Property.Place14);
            PropertyChange(Property.Place15);
            PropertyChange(Property.Place16);
            PropertyChange(Property.Place17);
            PropertyChange(Property.Place18);
            PropertyChange(Property.Place19);
            PropertyChange(Property.Place20);
        }

        # endregion

        # endregion

        public MainWindow MainWindow = App.ParentWindow;

        public Tournament Tournament { get { return MainWindow.Tournament; } }
        public TournamentDetail Detail { get { return Tournament.TournamentDetail; } }

        public Double PrizePool { get { return MainWindow.PrizePool; } }

        public bool IsChanceVisible { get { return Tournament.GameTypeEnum.In(eGameType.DoubleChance, eGameType.TripleChance); } }

        public Double Dotation { get { return MainWindow.Dotation; } }
        public Double League { get { return MainWindow.LeagueMoney; } }
        public Double Rake { get { return MainWindow.RakeMoney; } }
        public Double WinnerMoney { get { return MainWindow.WinMoney; } }

        public Double MoneyPool { get { return MainWindow.MoneyPool; } }

        public int[] Places { get { return IsAuto ? MainWindow.GetPlaces() : MainWindow.ManualPlaces.ToArray(); } set { } }

        public bool IsAuto { get { return App.ParentWindow.IsAuto; } set { App.ParentWindow.IsAuto = true; Refresh(); } }
        public bool IsManual { get { return !App.ParentWindow.IsAuto; } set { App.ParentWindow.IsAuto = false; Refresh(); } }

        public TournamentSettingsDlg()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void closeBtn_click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void okBtn_click(object sender, RoutedEventArgs e)
        {
        }

        private int _place01;
        public int Place01
        {
            get
            {
                if (IsAuto)
                    return Places[0];
                else
                    return _place01;
            }
            set
            {
                _place01 = value;
                Refresh();
            }
        }

        private int _place02;
        public int Place02
        {
            get
            {
                if (IsAuto)
                    return Places[1];
                else
                    return _place02;
            }
            set
            {
                _place02 = value;
                Refresh();
            }
        }

        private int _place03;
        public int Place03
        {
            get
            {
                if (IsAuto)
                    return Places[2];
                else
                    return _place03;
            }
            set
            {
                _place03 = value;
                Refresh();
            }
        }

        private int _place04;
        public int Place04
        {
            get
            {
                if (IsAuto)
                    return Places[3];
                else
                    return _place04;
            }
            set
            {
                _place04 = value;
                Refresh();
            }
        }

        private int _place05;
        public int Place05
        {
            get
            {
                if (IsAuto)
                    return Places[4];
                else
                    return _place05;
            }
            set
            {
                _place05 = value;
                Refresh();
            }
        }

        private int _place06;
        public int Place06
        {
            get
            {
                if (IsAuto)
                    return Places[5];
                else
                    return _place06;
            }
            set
            {
                _place06 = value;
                Refresh();
            }
        }

        private int _place07;
        public int Place07
        {
            get
            {
                if (IsAuto)
                    return Places[6];
                else
                    return _place07;
            }
            set
            {
                _place07 = value;
                Refresh();
            }
        }

        private int _place08;
        public int Place08
        {
            get
            {
                if (IsAuto)
                    return Places[7];
                else
                    return _place08;
            }
            set
            {
                _place08 = value;
                Refresh();
            }
        }
    }
}