using System.ComponentModel;
using System.Windows.Controls;
using PokerTimer.BusinessObjects.DataClasses;
using PokerTimer.BusinessObjects;
using System.Linq;
using System;

namespace PokerTimer.Win.Controls
{
    public partial class StructureLineCtrl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StructureDetail StructureDetail { get; set; }

        public StructureLineCtrl(StructureDetail structureDetail)
        {
            InitializeComponent();

            StructureDetail = structureDetail;
            DataContext = this;

            var x = Enum.GetValues(typeof(eStructureLineType)).Cast<eStructureLineType>().Select(t => new SelectableEnum<eStructureLineType>(t));
        }

        private void Button_DragOver(object sender, System.Windows.DragEventArgs e)
        {
        }
    }
}