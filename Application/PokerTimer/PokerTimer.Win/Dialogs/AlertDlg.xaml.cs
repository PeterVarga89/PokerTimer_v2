using System.Windows;

namespace PokerTimer.Win.Dialogs
{
    public partial class AlertDlg : Window
    {
        public AlertDlg(string text)
        {
            InitializeComponent();
            txtText.Text = text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}