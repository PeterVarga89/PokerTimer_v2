using System.Windows;

namespace PokerTimer.Win
{
    public static class UiExtensions
    {
        public static void SetVisibility(this UIElement element, bool boolean)
        {
            element.Visibility = boolean ? Visibility.Visible : Visibility.Collapsed;
        }

        public static Visibility ToVisibility(this bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}