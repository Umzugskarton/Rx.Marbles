using AIMarbles.Core;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Model;
using System.Reactive.Linq;

namespace AIMarbles.ViewModel
{
    public class ConnectionViewModel : CanvasObjectViewModelBase
    {
        private double _x1 = 0;
        private double _y1 = 0;
        private double _x2 = 0;
        private double _y2 = 0;

        private CanvasObjectViewModelBase? _from;
        private CanvasObjectViewModelBase? _to;

        public ActorId Id { get; } 

        // TODO Change to SetProperty
        public double X1 { get { return _x1; } set { _x1 = value; OnPropertyChanged(nameof(X1)); } }
        public double Y1 { get { return _y1; } set { _y1 = value; OnPropertyChanged(nameof(Y1)); } }

        public double X2 { get { return _x2; } set { _x2 = value; OnPropertyChanged(nameof(X2)); } }
        public double Y2 { get { return _y2; } set { _y2 = value; OnPropertyChanged(nameof(Y2)); } }


        public CanvasObjectViewModelBase? From { 
            get { return _from; } 
            set {
                _from = value;
                IsConnectionModeActive = true;
                SubscribeToFromCoordinateChanges(value);
            } 
        }
        public CanvasObjectViewModelBase? To { 
            get { return _to; } 
            set {
                _to = value;
                IsConnectionModeActive = false;
                SubscribeToToCoordinateChanges(value);
            } 
        }

        public ConnectionViewModel(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine)
            : base(canvasObjectService, marbleMachineEngine)
        {
            Id = new ActorId(Guid.NewGuid().ToString());
            // _animationService = new AnimationService();
            // _animationService.StartAnimation(this);
        }

        // has no effect
        protected override List<Type> _allowedConnectionsList() => [];

        private void SubscribeToFromCoordinateChanges(CanvasObjectViewModelBase? from)
        {
            if (from == null) { return;  }
            AddDisposable(
                from.WhenCoordinatesChange
                .CombineLatest(from.WhenViewDimensionsChange)
                    .Subscribe(bounds =>
                    {
                        X1 = bounds.First.X + bounds.Second.viewWidht / 2 ;
                        Y1 = bounds.First.Y + bounds.Second.viewHeight;
                    }
            ));
        }

        private void SubscribeToToCoordinateChanges(CanvasObjectViewModelBase? to)
        {
            if (to == null) { return;  }

            AddDisposable(
                to.WhenCoordinatesChange
                .CombineLatest(to.WhenViewDimensionsChange)
                    .Subscribe(bounds =>
                    {
                        X2 = bounds.First.X + bounds.Second.viewWidht / 2 ;
                        Y2 = bounds.First.Y;
                    }
            ));
        }

    }
}
