using AIMarbles.Core;
using AIMarbles.Core.Factory;
using AIMarbles.Core.Helper;
using AIMarbles.Core.Interface;
using AIMarbles.Core.Interface.Factory;
using AIMarbles.Core.Interface.Service;
using AIMarbles.Core.Service;
using AIMarbles.View;
using AIMarbles.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace AIMarbles
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>


    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICanvasObjectService, CanvasObjectService>(); 
            services.AddSingleton<IMarbleMachineManager, MarbleMachineManager>(); 
            services.AddSingleton<IMidiOutputService, MIDIOutputService>(); 
            services.AddSingleton<IMarbleMachineEngine, MarbleMachineEngine>(); 

            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<PalleteViewModel>();

            services.AddTransient<MainWindow>();
            services.AddTransient<PalleteView>();

            services.AddTransient(typeof(IViewModelFactory<>), typeof(ViewModelFactory<>));
            services.AddTransient<ChannelViewModel>();
            services.AddTransient<NoteViewModel>();
            services.AddTransient<MetronomViewModel>();
            services.AddTransient<ConnectionViewModel>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            ServiceProvider?.GetServices<ViewModelBase>().ToList()
                .ForEach(service => service.Dispose());
            (ServiceProvider as IDisposable)?.Dispose(); 
        }
    }
}
