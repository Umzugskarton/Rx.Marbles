using AIMarbles.Core.Interface.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace AIMarbles.Core.Factory
{
    public class ViewModelFactory<TViewModel> : IViewModelFactory<TViewModel> where TViewModel : class
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TViewModel Create()
        {
            return _serviceProvider.GetRequiredService<TViewModel>();
        }
    }
}
