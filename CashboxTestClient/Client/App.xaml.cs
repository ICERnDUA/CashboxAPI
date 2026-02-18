using Client.Interfaces;
using Client.Models;
using Client.ViewModels;
using Client.Views;
using Prism.Ioc;
using Prism.Unity;
using System.Configuration;
using System.Data;
using System.Windows;
namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<MainWindow>();
            containerRegistry.RegisterForNavigation<ServerConnectSettingsView, ServerConnectSettingsViewModel>();
            containerRegistry.Register<INetworkDAO, NetworkDAO>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }

}
