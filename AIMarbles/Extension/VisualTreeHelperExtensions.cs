using System.Windows;
using System.Windows.Media;

namespace AIMarbles.Extensions
{
    public static class VisualTreeHelperExtensions
    {
        public static T? FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            while (parent != null && parent is not T)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as T;
        }

    }
}
