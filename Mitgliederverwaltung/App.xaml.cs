using System;
using System.Windows;
using Anotar.Log4Net;
using log4net.Config;
using Microsoft.Practices.Unity;
using Mitgliederverwaltung.Database;
using Mitgliederverwaltung.ViewModel;
using Mitgliederverwaltung.Views;

namespace Mitgliederverwaltung
{
    /// <summary>
    ///     Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public const string AppID = "MVVMVEREIN";

        private MainWindow _mw;
        public static IUnityContainer UnityContainer => (IUnityContainer) Current.Properties["UnityContainer"];

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            XmlConfigurator.Configure();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(LogUnhandledException); ;

            var unityContainer = new UnityContainer();
            Properties["UnityContainer"] = unityContainer;
            Bootstrapper.ConfigureContainer(unityContainer);

            SQLiteDB.Init();

            _mw = unityContainer.Resolve<MainWindow>();
            _mw.Show();

            

        }

        public void Init()
        {
            InitializeComponent();
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogTo.FatalException("Unhandled exception occured", e.ExceptionObject as Exception);
        }
    }
}