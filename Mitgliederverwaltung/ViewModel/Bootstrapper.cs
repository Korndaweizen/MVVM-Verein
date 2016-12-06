using Anotar.Log4Net;
using Microsoft.Practices.Unity;
using Mitgliederverwaltung.Services;
using SettingsProviderNet;
using SingletonLifetime = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;

namespace Mitgliederverwaltung.ViewModel
{
    public class Bootstrapper
    {
        public Bootstrapper()
        {
            if (App.UnityContainer == null)
            {
                Container = new UnityContainer();
                ConfigureContainer(Container);
            }
            else
            {
                Container = App.UnityContainer;
            }
        }

        public IUnityContainer Container { get; }

        public static void ConfigureContainer(IUnityContainer container)
        {
            LogTo.Info("Configuring dependencies");
            container.RegisterType<ISettingsProvider, SettingsProvider>(new SingletonLifetime(),
                new InjectionFactory(f => new SettingsProvider(new RoamingAppDataStorage(App.AppID))));
            container.RegisterType<IDemoService, DemoService>();
            container.RegisterType<IOFDialogue, OFDialogue>();
            container.RegisterType<ISFDialogue, SFDialogue>();
        }
    }
}