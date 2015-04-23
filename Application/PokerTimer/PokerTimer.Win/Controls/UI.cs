using System.Windows;
using System.Windows.Controls;
using PokerTimer.BusinessObjects;
using PokerTimer.Win;


namespace Img.Crm5.ICE.WR.Calendar.Behaviors
{
    public class UI
    {
        #region IsVisible

        public static readonly DependencyProperty IsVisibleProperty =
        DependencyProperty.RegisterAttached("IsVisible",
                                            typeof(bool),
                                            typeof(UI),
                                            new PropertyMetadata(true, IsVisibleChanged));

        public static bool GetIsVisible(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsVisibleProperty);
        }

        public static void SetIsVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsVisibleProperty, value);
        }

        private static void IsVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            //obj.CastAs<UIElement>(element => element.SetVisibility(GetIsVisible(element)));
        }

        #endregion

        #region IsHidden

        public static readonly DependencyProperty IsHiddenProperty =
        DependencyProperty.RegisterAttached("IsHidden",
                                            typeof(bool),
                                            typeof(UI),
                                            new PropertyMetadata(false, IsHiddenChanged));

        public static bool GetIsHidden(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHiddenProperty);
        }

        public static void SetIsHidden(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHiddenProperty, value);
        }

        private static void IsHiddenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            //obj.CastAs<UIElement>(element => element.SetVisibility(!GetIsHidden(element)));
        }

        #endregion
    }
}