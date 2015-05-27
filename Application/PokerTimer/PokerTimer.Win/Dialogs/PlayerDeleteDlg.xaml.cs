using System;
using System.Collections.Generic;
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

namespace PokerTimer.Win.Dialogs
{
    public partial class PlayerDeleteDlg : Window
    {
        public bool IsOut = false;
        public bool IsDelete = false;

        public PlayerDeleteDlg()
        {
            InitializeComponent();
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            IsOut = true;
            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            IsDelete = true;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
