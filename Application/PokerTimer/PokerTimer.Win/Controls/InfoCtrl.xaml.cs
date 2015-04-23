using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using PokerTimer.BusinessObjects;

namespace PokerTimer.Win.Controls
{
    public partial class InfoCtrl : UserControl, INotifyPropertyChanged
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
            Message,
            Value
        }

        # endregion

        private static void MessagePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            InfoCtrl uc1 = (InfoCtrl)dependencyObject;
            uc1.Message = (string)dependencyPropertyChangedEventArgs.NewValue;
        }

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(InfoCtrl), new PropertyMetadata(new PropertyChangedCallback(MessagePropertyChanged)));
        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
                PropertyChange(Property.Message);
            }
        }

        private static void TextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            InfoCtrl uc1 = (InfoCtrl)dependencyObject;
            uc1.Text = (string)dependencyPropertyChangedEventArgs.NewValue;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(InfoCtrl), new PropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
                PropertyChange(Property.Value);
            }
        }

        public InfoCtrl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}