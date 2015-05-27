using System.Windows;

namespace PokerTimer.Win.Dialogs
{
    public partial class StringInputDlg : Window
    {
        public string InputText { get; set; }

        public StringInputDlg()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (InputText == string.Empty)
            {
                return;
            }
            this.Close();
        }
    }
}