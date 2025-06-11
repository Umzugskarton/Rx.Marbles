using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMarbles.Core.Interface.Factory
{
    public interface IViewModelFactory<TViewModel> where TViewModel : class
    {
        TViewModel Create();
    }
}
