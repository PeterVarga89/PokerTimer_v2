using System;
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

        public static void Alert(string text, Action returnCallBack = null)
        {
            Dialogs.AlertDlg dlg = new Dialogs.AlertDlg(text);
            dlg.ShowDialog();

            if (returnCallBack != null)
            {
                returnCallBack();
            }
        }
    }
}