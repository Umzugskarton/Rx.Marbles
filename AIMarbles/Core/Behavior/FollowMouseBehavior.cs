using AIMarbles.Extensions;
using AIMarbles.ViewModel;
using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AIMarbles.Core.Behavior
{
    public class FollowMouseBehavior : Behavior<FrameworkElement>
    {
        // This attached property will store the CanvasBehaviorState for each element
        private static readonly DependencyProperty CurrentCanvasBehaviorStateProperty =
            DependencyProperty.RegisterAttached("CurrentCanvasBehaviorState", typeof(CanvasBehaviorState), typeof(FollowMouseBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty ActiveConnectionProperty =
        DependencyProperty.Register("ActiveConnection", typeof(ConnectionViewModel), typeof(FollowMouseBehavior), new PropertyMetadata(null));


        public ConnectionViewModel ActiveConnection
        {
            get { return (ConnectionViewModel)GetValue(ActiveConnectionProperty); }
            set { SetValue(ActiveConnectionProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseDown += OnMouseDown;
            AssociatedObject.MouseUp += OnMouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseDown -= OnMouseDown;
            AssociatedObject.MouseUp -= OnMouseUp;
        }


        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!(sender is FrameworkElement element && element.DataContext is ConnectionViewModel item)) return;

        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!(sender is FrameworkElement element && element.DataContext is ConnectionViewModel item)) return;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (ActiveConnection == null) { return; }
            Point currentMousePosition = e.GetPosition(AssociatedObject);
            ActiveConnection.X2 = currentMousePosition.X;
            ActiveConnection.Y2 = currentMousePosition.Y;
        }

    }
}
