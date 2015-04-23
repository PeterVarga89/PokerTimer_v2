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
    public partial class SelectStructureDlg : Window
    {
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

        public List<Structure> Structures { get; set; }

        # endregion

        public SelectStructureDlg()
        {
            InitializeComponent();
            BindListBox();
        }

        private void BindListBox()
        {
            var worker = new BackgroundWorker();
            worker.DoWork += delegate
            {
                Structures = BusinessObjects.Data.StructureData.GetList(eConnectionString.Online);
            };

            worker.RunWorkerCompleted += delegate
            {
                lbxTournaments.ItemsSource = Structures;
                IsBusy = false;
            };

            IsBusy = true;
            worker.RunWorkerAsync();
        }

        private void btnJoinClick(object sender, RoutedEventArgs e)
        {
            if (lbxTournaments.SelectedItem.IsNotNull())
            {
                App.ParentWindow.Structure = lbxTournaments.SelectedItem as Structure;
                this.Close();
            }
        }
    }
}
