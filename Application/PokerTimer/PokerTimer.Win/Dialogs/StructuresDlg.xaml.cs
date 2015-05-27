using System;
using System.ComponentModel;
using System.Windows;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.Win.Dialogs
{
    public partial class StructuresDlg : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StructuresDlg()
        {
            InitializeComponent();
            DataContext = this;
            BindData();
        }

        private void BindData()
        {
            lbxStructures.ItemsSource = BusinessObjects.Data.StructureData.GetList(eConnectionString.Online);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lbxStructures.SelectedItem != null)
            {
                var id = (lbxStructures.SelectedItem as Structure).StructureId;

                var dlg = new Dialogs.StructureEditDlg(id) { Structure = (lbxStructures.SelectedItem as Structure) };
                dlg.Closed += delegate { BindData(); };
                dlg.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbxStructures.SelectedItem != null)
            {
                var id = (lbxStructures.SelectedItem as Structure).StructureId;
                BusinessObjects.Data.StructureData.Delete(eConnectionString.Online, id);
                BindData();
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Dialogs.StructureEditDlg(Guid.Empty) { Structure = new Structure() };
            dlg.Closed += delegate { BindData(); };
            dlg.ShowDialog();
        }

        private void btnUse_Click(object sender, RoutedEventArgs e)
        {
            if (lbxStructures.SelectedItem != null)
            {
                App.ParentWindow.Structure = lbxStructures.SelectedItem as Structure;
            }
        }
    }
}