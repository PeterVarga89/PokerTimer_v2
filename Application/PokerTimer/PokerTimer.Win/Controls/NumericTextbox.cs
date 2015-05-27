using System.ComponentModel;
using System.Windows.Controls;
using PokerTimer.BusinessObjects;

namespace PokerTimer.Win.Controls
{
    public class NumericTextbox : TextBox, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public NumericTextbox()
        {
            this.KeyUp += delegate 
            {
                PropertyChanged.Raise(() => Text); 
            };
        }
    }
}