using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PokerTimer.BusinessObjects;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.Win.Dialogs
{
    public partial class PlayerListDlg : Window
    {
        public PlayerListDlg()
        {
            InitializeComponent();
            this.KeyUp += PlayerListDialog_KeyUp;
            lbPlayers.ItemsSource = App.ParentWindow.PlayerList;
        }

        private void PlayerListDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F3)
            {
                this.Close();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            App.ParentWindow.Refresh();
        }

        private void btnRebuyDown_Click(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            if (result.ReBuyCount > 0)
                result.ReBuyCount--;

            App.ParentWindow.Refresh();
        }

        private void btnRebuyUp_Click(object sender, RoutedEventArgs e)
        {
            if(!App.ParentWindow.Tournament.GameTypeEnum.In(eGameType.FreezeOut))
            {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            var dlg = new Dialogs.ReBuyDlg(result);
            dlg.ShowDialog();
            this.Focus();

            App.ParentWindow.Refresh();
            }
        }

        private void btnAddOnDown(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            if (result.AddOnCount > 0)
                result.AddOnCount--;

            App.ParentWindow.Refresh();
        }

        private void btnAddOnUp(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);
            if (result.AddOnCount == 0)
                result.AddOnCount++;

            App.ParentWindow.Refresh();
        }

        private void btnPokerDown(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            if (result.PokerCount > 0)
                result.PokerCount--;
        }

        private void btnPokerUp(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            result.PokerCount++;
        }

        private void btnSFlushDown(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            if (result.StraightFlushCount > 0)
                result.StraightFlushCount--;
        }

        private void btnSFlushUp(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            result.StraightFlushCount++;
        }

        private void btnRFlushDown(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            if (result.RoyalFlushCount > 0)
                result.RoyalFlushCount--;
        }

        private void btnRFlushUp(object sender, RoutedEventArgs e)
        {
            var id = (sender as Button).Tag.ToString().ToGuid();

            var result = App.ParentWindow.PlayerList.SingleOrDefault(p => p.TournamentResultId == id);

            result.RoyalFlushCount++;
        }
    }
}