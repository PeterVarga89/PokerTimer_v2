using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.Win.Dialogs
{
    public partial class SelectTournamentDlg : Window, INotifyPropertyChanged
    {
        public SelectTournamentDlg()
        {
            InitializeComponent();
            DataContext = this;
            BindListBox();
            this.Closing += SelectTournamentDlg_Closing;
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
            IsSelected
        }

        private void RefreshProps()
        {
            PropertyChange(Property.IsSelected);
        }

        # endregion

        # region Props

        private bool IsBusy
        {
            get
            {
                return busy.IsBusy;
            }
            set
            {
                busy.IsBusy = value;
            }
        }

        private List<Tournament> TournamentList { get; set; }

        public Tournament SelectedTournament { get; set; }

        public Structure SelectedStructure { get; set; }

        public bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RefreshProps();
            }
        }

        # endregion

        private void BindListBox()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                TournamentList = BusinessObjects.Data.TournamentData.GetFutureList(eConnectionString.Online, 10);
            };

            worker.RunWorkerCompleted += delegate
            {
                lbxTournaments.ItemsSource = TournamentList.OrderBy(t => t.Date).ToList();
                IsBusy = false;
            };

            IsBusy = true;
            worker.RunWorkerAsync();
        }

        # region Events

        private void btnJoinClick(object sender, RoutedEventArgs e)
        {
            if (lbxTournaments.SelectedItem.IsNull())
                return;

            SelectedTournament = lbxTournaments.SelectedItem as Tournament;

            App.ParentWindow.Tournament = SelectedTournament;

            if(SelectedTournament.Structure.IsNull())
            {
                Dialogs.SelectStructureDlg dlg = new SelectStructureDlg();
                dlg.ShowDialog();
            }
            else
            {
                App.ParentWindow.Structure = App.ParentWindow.Tournament.Structure;
            }

            this.Close();
        }

        private void SelectTournamentDlg_Closing(object sender, CancelEventArgs e)
        {
            if (SelectedTournament.IsNull())
                Environment.Exit(0);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        # endregion

        private void lbxTournaments_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            IsSelected = lbxTournaments.SelectedItem.IsNotNull();
            RefreshProps();
        }
    }
}
