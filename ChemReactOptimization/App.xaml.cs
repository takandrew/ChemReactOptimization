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
            builder.RegisterType<AuthorizationWindowViewModel>().AsSelf();

            var container = builder.Build();
            var authorizationWindowViewModel = container.Resolve<AuthorizationWindowViewModel>();
            var authorizationWindow = new AuthorizationWindow { DataContext = authorizationWindowViewModel };
            authorizationWindow.Show();
        }
    }
}
