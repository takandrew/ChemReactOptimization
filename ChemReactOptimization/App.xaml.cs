using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ChemReactOptimization.ViewModel;
using Autofac;

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
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            var container = builder.Build();
            var mainWindowViewModel = container.Resolve<MainWindowViewModel>();
            var mainWindow = new MainWindow { DataContext = mainWindowViewModel };
            mainWindow.Show();
        }
    }
}
