using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AIMarbles.Core.Behavior
{
    class CanvasBehaviorState
    {
        public Point StartPoint { get; set; }
        public CanvasObjectViewModelBase? BehaviorObject { get; set; }
        public Canvas? ParentCanvas { get; set; } 
    }
}
