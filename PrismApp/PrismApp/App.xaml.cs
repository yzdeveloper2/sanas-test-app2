using System;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using PrismApp.Modules.Users;
using PrismApp.Services;
using PrismApp.Services.Interfaces;
using PrismApp.Views;

namespace PrismApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        containerRegistry.RegisterSingleton<IUserService>(() => new UserService(GetUserServiceUrl()));
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<UsersModule>();
    }

    private static string GetUserServiceUrl()
    {
        var url = System.Configuration.ConfigurationManager.AppSettings.Get("UserServiceUrl");
        return url ?? throw new ArgumentException("UserServiceUrl - is missing");
    }
}