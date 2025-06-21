using System.Windows;
using System.Windows.Media;

namespace AIMarbles.Extensions
{
    public static class VisualTreeHelperExtensions
    {
        public static bool FindParent<T>(this DependencyObject child, out T? result) where T : DependencyObject
        {
            result = null;
            var parent = VisualTreeHelper.GetParent(child);
            while (parent != null && parent is not T)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent is T tParent)
            {
                result = tParent;
                return true;
            }

            return false;
        }

    }
}
