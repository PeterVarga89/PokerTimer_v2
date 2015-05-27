using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.Win.Dialogs
{
    public partial class StructureEditDlg : Window, INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                PropertyChanged.Raise(() => IsBusy);
            }
        }

        public Structure Structure { get; set; }

        private ObservableCollection<StructureDetail> _details;
        public ObservableCollection<StructureDetail> Details
        {
            get
            {
                return _details;
            }
            set
            {
                _details = value;
                PropertyChanged.Raise(() => Details);
            }
        }

        public StructureEditDlg(Guid structureId)
        {
            InitializeComponent();
            DataContext = this;
            Details = new ObservableCollection<StructureDetail>();
            BindData(structureId);

            Details.CollectionChanged += delegate { Recalculate(); };
        }

        private void Recalculate()
        {
            int number = 0;
            int level = 1;

            foreach (var d in Details)
            {
                d.Number = number;
                d.Level = level;

                d.RefreshVisibility();
                d.PropertyChanged += delegate
                {
                    //Recalculate();
                };

                number++;
                if (d.TypeEnum == eStructureLineType.Level)
                {
                    level++;
                }
            }
        }

        private void BindData(Guid structureId)
        {
            if (structureId != Guid.Empty)
            {
                var details = BusinessObjects.Data.StructureData.GetDetailsByStrucureId(BusinessObjects.eConnectionString.Online, structureId).ToList();
                Details = details.ToObservableCollection<StructureDetail>();
            }

            Recalculate();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Structure.StructureId == Guid.Empty)
            {
                var inputDlg = new Dialogs.StringInputDlg();
                inputDlg.Closing += delegate
                {
                    Structure.Name = inputDlg.InputText;
                };
                inputDlg.ShowDialog();
            }

            var bgw = new BackgroundWorker();

            bgw.DoWork += delegate
            {
                BusinessObjects.Data.StructureData.InsertOrUpdate(BusinessObjects.eConnectionString.Online, Structure, Details.ToList());
            };

            bgw.RunWorkerCompleted += delegate
            {
                IsBusy = false;
                if ((sender as Button).Tag != null)
                {
                    this.Close();
                }
            };

            bgw.RunWorkerAsync();
            IsBusy = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            Recalculate();
            var bgw = new BackgroundWorker();

            bgw.DoWork += delegate
            {
                BusinessObjects.Data.StructureData.InsertOrUpdate(BusinessObjects.eConnectionString.Online, Structure.GetCopy(), Details.ToList(), true);
            };

            bgw.RunWorkerCompleted += delegate
            {
                IsBusy = false;
                if ((sender as Button).Tag != null)
                {
                    this.Close();
                }
            };

            var inputDlg = new Dialogs.StringInputDlg();
            inputDlg.Closing += delegate
            {
                Structure.Name = inputDlg.InputText;
                bgw.RunWorkerAsync();
            };
            inputDlg.ShowDialog();
            IsBusy = true;
        }

        private void btnNewRow_Click(object sender, RoutedEventArgs e)
        {
            var index = lbxItems.SelectedIndex < 0 ? 0 : lbxItems.SelectedIndex;

            var item = new StructureDetail() { StructureId = Structure.StructureId, StructureDetailId = Guid.NewGuid(), TypeEnum = eStructureLineType.Level };
            Details.Insert(index, item);
            lbxItems.ScrollIntoView(item);
        }

        private void btnDeleteRow_Click(object sender, RoutedEventArgs e)
        {
            if (lbxItems.SelectedItem != null)
            {
                Details.Remove(lbxItems.SelectedItem as StructureDetail);
            }

            Recalculate();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var structDetail = Details.SingleOrDefault(d => d.StructureDetailId == id);

            var index = Details.IndexOf(structDetail);

            Details.Remove(structDetail);
            Details.Insert(index - 1, structDetail);
            Recalculate();
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var structDetail = Details.SingleOrDefault(d => d.StructureDetailId == id);

            var index = Details.IndexOf(structDetail);

            Details.Remove(structDetail);
            Details.Insert(index + 1, structDetail);
            Recalculate();
        }

        private void structDelete_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();
            var structDetail = Details.SingleOrDefault(d => d.StructureDetailId == id);
            Details.Remove(structDetail);
        }
    }
}