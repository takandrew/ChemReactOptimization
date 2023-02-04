using System.Windows;
using ChemReactOptimization.ViewModel;
using Autofac;
using ChemReactOptimization.Data;

namespace ChemReactOptimization
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EFMethods>().AsSelf();
            builder.RegisterType<EFTasks>().AsSelf();
            builder.RegisterType<EFUsers>().AsSelf();
            builder.RegisterType<ChemReactContext>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            var container = builder.Build();
            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
            var mainWindow = new MainWindow { DataContext = mainWindowViewModel };
            mainWindow.Show();
        }
    }
}
