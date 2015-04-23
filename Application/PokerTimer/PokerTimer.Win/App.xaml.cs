using System.Windows;

namespace PokerTimer.Win
{
    public partial class App : Application
    {
        public App()
            : base()
        {
        }

        public static MainWindow ParentWindow
        {
            get;
            set;
        }
    }
}