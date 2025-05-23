namespace AIMarbles.Core
{
    abstract internal class CanvasObjectViewModelBase : ViewModelBase
    {

        private double _x = 0;
        private double _y = 0;

        private string _name = "";
        private bool _isSelected = false;

        private List<CanvasObjectViewModelBase> _connectedObjects = new List<CanvasObjectViewModelBase>();

        //Overwride this in the derived class to set the allowed connections
        private List<CanvasObjectViewModelBase> _allowedConnectionsList() { return new List<CanvasObjectViewModelBase>(); }
        public CanvasObjectViewModelBase()
        {
            Name = Guid.NewGuid().ToString();
        }
        public CanvasObjectViewModelBase(double x, double y)
        {
            X = x;
            Y = y;
            Name = Guid.NewGuid().ToString();
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public List<CanvasObjectViewModelBase> ConnectedObjects
        {
            get { return _connectedObjects; }
        }

        public List<CanvasObjectViewModelBase> AllowedConnectionsList
        {
            get { return _allowedConnectionsList(); }
        }

        public void AddConnected(CanvasObjectViewModelBase item)
        {
            if (item == null) return;
            if (_connectedObjects.Contains(item)) return;
            if (!isConnectionAllowed(item)) return;
            _connectedObjects.Add(item);
            OnPropertyChanged(nameof(ConnectedObjects));
        }

        public void RemoveConnected(CanvasObjectViewModelBase item)
        {
            if (item == null) return;
            if (!_connectedObjects.Contains(item)) return;
            _connectedObjects.Remove(item);
            OnPropertyChanged(nameof(ConnectedObjects));
        }

        public double X { get { return _x; } set { _x = value; OnPropertyChanged(nameof(X)); } }
        public double Y { get { return _y; } set { _y = value; OnPropertyChanged(nameof(Y)); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        private bool isConnectionAllowed(CanvasObjectViewModelBase connector) { return _allowedConnectionsList().Contains(connector); }

    }
}
