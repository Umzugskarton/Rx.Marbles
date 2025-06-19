using System.Windows;

namespace AIMarbles.Core.Helper;
public static class ViewDimensionHelper
{
    public static readonly DependencyProperty PassDimensionsToViewModelProperty =
        DependencyProperty.RegisterAttached(
            "PassDimensionsToViewModel",
            typeof(bool),
            typeof(ViewDimensionHelper),
            new PropertyMetadata(false, OnPassDimensionsToViewModelChanged));

    public static bool GetPassDimensionsToViewModel(DependencyObject obj)
    {
        return (bool)obj.GetValue(PassDimensionsToViewModelProperty);
    }

    public static void SetPassDimensionsToViewModel(DependencyObject obj, bool value)
    {
        obj.SetValue(PassDimensionsToViewModelProperty, value);
    }

    private static void OnPassDimensionsToViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement view && (bool)e.NewValue)
        {
            view.Loaded += View_LoadedOrSizeChanged;
            view.SizeChanged += View_LoadedOrSizeChanged;
        }
        else if (d is FrameworkElement oldView && !(bool)e.NewValue)
        {
            oldView.Loaded -= View_LoadedOrSizeChanged;
            oldView.SizeChanged -= View_LoadedOrSizeChanged;
        }
    }

    private static void View_LoadedOrSizeChanged(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement view || view.DataContext is not CanvasObjectViewModelBase viewModel) { return; }
        viewModel.UpdateViewDimensions(view.ActualWidth, view.ActualHeight);

    }
}
