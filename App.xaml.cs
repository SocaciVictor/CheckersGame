using CheckersGame.Core;
using CheckersGame.Services;
using CheckersGame.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CheckersGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainMenuViewModel>()
            });
            services.AddSingleton<MainMenuViewModel>();
            services.AddSingleton<LoadGameViewModel>();
            services.AddSingleton<AboutViewModel>();
            services.AddSingleton<StatisticsViewModel>();
            services.AddSingleton<PlayViewModel>();
            services.AddSingleton<GameViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();


            services.AddSingleton<Func<Type, ViewModel>>(servicesProvider => viewModelType => (ViewModel)servicesProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var navigationService = _serviceProvider.GetRequiredService<INavigationService>();
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            navigationService.NavigateTo<MainMenuViewModel>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
