using AIMarbles.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AIMarbles.Core.Behavior
{
    class DragState
    {
        public Point StartPoint { get; set; }
        public DraggableView? DraggingItem { get; set; }
        public Canvas? ParentCanvas { get; set; } 
    }

    public static class DragBehavior
    {
        // This attached property will store the DragState for each element
        private static readonly DependencyProperty CurrentDragStateProperty =
            DependencyProperty.RegisterAttached("CurrentDragState", typeof(DragState), typeof(DragBehavior), new PropertyMetadata(null));


        public static bool GetIsDraggable(DependencyObject obj) => (bool)obj.GetValue(IsDraggableProperty);
        public static void SetIsDraggable(DependencyObject obj, bool value) => obj.SetValue(IsDraggableProperty, value);

        public static readonly DependencyProperty IsDraggableProperty =
            DependencyProperty.RegisterAttached("IsDraggable", typeof(bool), typeof(DragBehavior), new PropertyMetadata(false, OnIsDraggableChanged));

        private static void OnIsDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element)) return;

            if ((bool)e.NewValue)
            {
                element.PreviewMouseDown += OnPreviewMouseDown;
                element.PreviewMouseMove += OnPreviewMouseMove;
                element.PreviewMouseUp += OnPreviewMouseUp;
            }
            else
            {
                element.PreviewMouseDown -= OnPreviewMouseDown;
                element.PreviewMouseMove -= OnPreviewMouseMove;
                element.PreviewMouseUp -= OnPreviewMouseUp;
                // Clean up the attached state when behavior is removed
                element.ClearValue(CurrentDragStateProperty);
            }
        }

        private static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement element && element.DataContext is DraggableView item)) return;

            var dragState = new DragState
            {
                DraggingItem = item,
                StartPoint = e.GetPosition(element)
            };

            var contentPresenter = VisualTreeHelper.GetParent(element) as ContentPresenter;
            if (contentPresenter != null)
            {
                dragState.ParentCanvas = VisualTreeHelper.GetParent(contentPresenter) as Canvas;
            }

            element.SetValue(CurrentDragStateProperty, dragState);
            element.CaptureMouse(); // Crucial for continuous dragging even if mouse leaves element

            Trace.WriteLine($"Drag started for {item.Name}. StartPoint (relative to item): {dragState.StartPoint}");
        }

        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!(sender is FrameworkElement element)) return;

            // Retrieve the instance-specific drag state
            var dragState = element.GetValue(CurrentDragStateProperty) as DragState;
            if (dragState == null || dragState.DraggingItem == null || dragState.ParentCanvas == null)
            {
                return;
            }

            DraggableView draggingItem = dragState.DraggingItem;
            Canvas parentCanvas = dragState.ParentCanvas;

            // Get current mouse position relative to the parent Canvas
            Point currentCanvasMousePosition = e.GetPosition(parentCanvas);

            double newX = currentCanvasMousePosition.X - dragState.StartPoint.X;
            double newY = currentCanvasMousePosition.Y - dragState.StartPoint.Y;

            double itemWidth = element.ActualWidth;
            double itemHeight = element.ActualHeight;

            double canvasWidth = parentCanvas.ActualWidth;
            double canvasHeight = parentCanvas.ActualHeight;

            if (canvasWidth <= 0 || canvasHeight <= 0)
            {
                Trace.WriteLine($"WARNING: Canvas ActualWidth ({canvasWidth}) or ActualHeight ({canvasHeight}) is zero. Cannot constrain movement. Item: {draggingItem.Name}. Check XAML layout for ItemsControl and its parent.");
                return; // Prevent movement if canvas size is unknown
            }

            // Clamp the new position within canvas bounds
            draggingItem.X = Math.Max(0, Math.Min(newX, canvasWidth - itemWidth));
            draggingItem.Y = Math.Max(0, Math.Min(newY, canvasHeight - itemHeight));

            Trace.WriteLine($"Item {draggingItem.Name} moved to ({draggingItem.X:F2}|{draggingItem.Y:F2}) on Canvas ({canvasWidth:F2}|{canvasHeight:F2})");
        }

        private static void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is FrameworkElement element)) return;

            element.ReleaseMouseCapture();
            element.ClearValue(CurrentDragStateProperty); // Clear state when drag ends
            Trace.WriteLine("Drag ended.");
        }
    }
}
