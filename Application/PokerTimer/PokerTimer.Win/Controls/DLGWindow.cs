using System.Windows;
using System.Windows.Controls;

namespace PokerTimer.Win.Controls
{
    public class DLGWindow : Window
    {
        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);
            this.Owner = App.ParentWindow;

            
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            
        }
    }
}