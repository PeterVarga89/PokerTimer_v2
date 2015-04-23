using System;
using System.ComponentModel;
using System.Windows;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.Win.Dialogs
{
    public partial class ReBuyDlg : Window, INotifyPropertyChanged
    {
        public TournamentResult TournamentResult { get; set; }

        # region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            Sum,
            Stack,
            BonusStack,
            StackTotal
        }

        private void RefreshValues()
        {
            PropertyChange(Property.BonusStack);
            PropertyChange(Property.StackTotal);
            PropertyChange(Property.Stack);
            PropertyChange(Property.Sum);
        }

        # endregion

        public bool IsRebuyEnabled { get { return App.ParentWindow.Tournament.GameTypeEnum.In(eGameType.FreeRoll, eGameType.RebuyLimited, eGameType.RebuyUnlimited); } }

        public bool IsAddOnEnabled { get { return App.ParentWindow.Tournament.GameTypeEnum.In(eGameType.FreeRoll, eGameType.RebuyLimited, eGameType.RebuyUnlimited); } }

        public bool IsDoubleChanceEnabled { get { return App.ParentWindow.Tournament.GameTypeEnum.In(eGameType.DoubleChance, eGameType.TripleChance); } }

        public bool IsTripleChanceEnabled { get { return App.ParentWindow.Tournament.GameTypeEnum.In(eGameType.TripleChance); } }

        private bool _isSingleRebuy;

        public bool IsSingleRebuy
        {
            get
            {
                return _isSingleRebuy;
            }
            set
            {
                _isSingleRebuy = value;
                RefreshValues();
            }
        }

        private bool _isDoubleRebuy;

        public bool IsDoubleRebuy
        {
            get
            {
                return _isDoubleRebuy;
            }
            set
            {
                _isDoubleRebuy = value;
                RefreshValues();
            }
        }

        private bool _isAddOn;

        public bool IsAddOn
        {
            get
            {
                return _isAddOn;
            }
            set
            {
                _isAddOn = value;
                RefreshValues();
            }
        }

        private bool _isDoubleChance;

        public bool IsDoubleChance
        {
            get
            {
                return _isDoubleChance;
            }
            set
            {
                _isDoubleChance = value;
                RefreshValues();
            }
        }

        private bool _isTripleChance;

        public bool IsTripleChance
        {
            get
            {
                return _isTripleChance;
            }
            set
            {
                _isTripleChance = value;
                RefreshValues();
            }
        }

        public int Sum
        {
            get
            {
                int rebuyPrize = App.ParentWindow.Tournament.TournamentDetail.ReBuyPrize;
                int addOnPrize = App.ParentWindow.Tournament.TournamentDetail.AddOnPrize;

                if (IsRebuyEnabled)
                {
                    return (IsSingleRebuy ? rebuyPrize : 2 * rebuyPrize) + (IsAddOn ? addOnPrize : 0);
                }
                else if (IsDoubleChanceEnabled)
                {
                    return (IsDoubleChance ? rebuyPrize : 0) + (IsTripleChance ? addOnPrize : 0);
                }
                else
                {
                    throw new NotImplementedException("SUM is not able to calculate....");
                }
            }
        }

        public Double Stack
        {
            get
            {
                int rebuyStack = App.ParentWindow.Tournament.TournamentDetail.ReBuyStack;
                int addOnStack = App.ParentWindow.Tournament.TournamentDetail.AddOnStack;

                if (IsRebuyEnabled)
                {
                    return (IsSingleRebuy ? rebuyStack : 2 * rebuyStack) + (IsAddOn ? addOnStack : 0);
                }
                else if (IsDoubleChanceEnabled)
                {
                    return (IsDoubleChance ? rebuyStack : 0) + (IsTripleChance ? addOnStack : 0);
                }
                else
                {
                    return rebuyStack;
                }
            }
        }

        public Double BonusStack
        {
            get
            {
                if (IsRebuyEnabled)
                {
                    return (IsDoubleRebuy || IsAddOn || (IsSingleRebuy && IsAddOn)) ? Stack * (0.20) : 0;
                }
                else if (IsDoubleChanceEnabled)
                {
                    return (IsDoubleChance || IsTripleChance) ? Stack * (0.20) : 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Double StackTotal { get { return Stack + BonusStack; } }

        public ReBuyDlg(TournamentResult tournamentResult)
        {
            TournamentResult = tournamentResult;

            InitializeComponent();
            DataContext = this;
            IsSingleRebuy = true;
        }

        private void okBtn_click(object sender, RoutedEventArgs e)
        {
            if (IsAddOn)
                TournamentResult.AddOnCount++;

            if (IsSingleRebuy)
                TournamentResult.ReBuyCount++;

            if (IsDoubleRebuy)
            {
                TournamentResult.ReBuyCount++;
                TournamentResult.ReBuyCount++;
            }

            TournamentResult.BonusStackTotal = TournamentResult.BonusStackTotal + (int)BonusStack;

            this.Close();
        }

        private void closeBtn_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}