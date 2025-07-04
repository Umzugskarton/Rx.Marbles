﻿using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Reactive.Bindings;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace AIMarbles.Core
{
    // TODO tear this class down into smaller components
    public abstract partial class CanvasObjectViewModelBase : ViewModelBase
    {
        protected readonly ICanvasObjectService _canvasObjectService;

        private ActorId _actorId;
        private double _x = 0;
        private double _y = 0;
        private double _viewWidth;
        private double _viewHeight;
        private bool _isSelected = false;
        private string _name = "";
        private readonly ReactiveProperty<double> _xReactive;
        private readonly ReactiveProperty<double> _yReactive;
        private readonly ReactiveProperty<double> _viewWidthReactive;
        private readonly ReactiveProperty<double> _viewHeightReactive;
        private readonly ReactiveProperty<bool> _isSelectedReactive;

        [ObservableProperty]
        private bool _isConnectionModeActiveByType = false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(InitiateLinkCommand))]
        private bool _isConnectionModeActive = false;

        public ActorId ActorId { get { return _actorId; } }
        public double ViewWidth
        {
            get => _viewWidth;
            set
            {
                if (SetProperty(ref _viewWidth, value))
                {
                    _viewWidthReactive.Value = value;
                }
            }
        }

        public double ViewHeight
        {
            get => _viewHeight;
            set
            {
                if (SetProperty(ref _viewHeight, value))
                {
                    _viewHeightReactive.Value = value;
                }
            }
        }

        public IObservable<(double viewWidht, double viewHeight)> WhenViewDimensionsChange => _viewWidthReactive.CombineLatest(
            _viewHeightReactive,
            (width, height) => (Width: width, Height: height)
        );

        public IObservable<(double X, double Y)> WhenCoordinatesChange => _xReactive.CombineLatest(
            _yReactive,
            (x, y) => (X: x, Y: y)
        );

        [RelayCommand(CanExecute = nameof(CanInitiateLink))]
        private void InitiateLink()
        {
            Trace.WriteLine($"Initiaing Link for canvasObject {Name}");
            _canvasObjectService.EnterConnectionMode(this);
        }

        private bool CanInitiateLink()
        {
            return !IsConnectionModeActive;
        }

        [RelayCommand]
        private void RegisterLink()
        {
            Trace.WriteLine($"Registering Link with canvasObject {Name}");
            _canvasObjectService.RegisterLink(this);
        }

        //Overwride this in the derived class to set the allowed connections
        protected abstract List<Type> _allowedConnectionsList();
        public CanvasObjectViewModelBase(ICanvasObjectService canvasObjectService, IMarbleMachineEngine marbleMachineEngine)
        {
            _canvasObjectService = canvasObjectService;
            _actorId = new ActorId();
            Name = Guid.NewGuid().ToString();
            X = 0;
            Y = 0;

            _xReactive = new ReactiveProperty<double>(X);
            _yReactive = new ReactiveProperty<double>(Y);
            _viewWidthReactive = new ReactiveProperty<double>(0);
            _viewHeightReactive = new ReactiveProperty<double>(0);
            _isSelectedReactive = new ReactiveProperty<bool>(false);
            SubscribeToConnectionModeState();
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (SetProperty(ref _isSelected, value))
                {
                    _x = _xReactive.Value;
                }
            }
        }

        public List<Type> AllowedConnectionsList
        {
            get { return _allowedConnectionsList(); }
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (SetProperty(ref _x, value))
                {
                    _xReactive.Value = value;
                }
            }
        }

        public double Y
        {
            get { return _y; }
            set
            {
                if (SetProperty(ref _y, value))
                {
                    _yReactive.Value = value;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        public void UpdateViewDimensions(double width, double height)
        {
            ViewWidth = width;
            ViewHeight = height;
            Trace.WriteLine($"ViewModel: View dimensions updated to W:{ViewWidth}, H:{ViewHeight}");
        }

        public bool isConnectionAllowed(Type connectorType) { return AllowedConnectionsList.Contains(connectorType); }

        private void SubscribeToConnectionModeState()
        {
            AddDisposables([
                        _canvasObjectService.SubscribeToIsConnectionModeActiveByTypeState(GetType(), isActive => {
                            Trace.WriteLine($"Is Active For Type {GetType()}: {isActive}");
                            IsConnectionModeActiveByType = isActive;
                        }),

                        _canvasObjectService.SubscribeToIsConnectionModeActiveState( isActive => {
                            Trace.WriteLine($"Is Active {isActive}");
                            IsConnectionModeActive = isActive;
                        })
                    ]
                );
        }
    }
}
