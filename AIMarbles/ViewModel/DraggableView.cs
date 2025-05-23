using AIMarbles.Core;

namespace AIMarbles.ViewModel
{
    abstract internal class DraggableView : ViewModelBase
    {
        private bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public DraggableView()
        {
            Name = Guid.NewGuid().ToString();
        }
        public DraggableView(double x, double y)
        {
            X = x;
            Y = y;
            Name = Guid.NewGuid().ToString();
        }
        private string _name = "";

        private double _x = 0;
        private double _y = 0;

        public double X { get { return _x; } set { _x = value; OnPropertyChanged(nameof(X)); } }
        public double Y { get { return _y; } set { _y = value; OnPropertyChanged(nameof(Y)); } }
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

    }
}
