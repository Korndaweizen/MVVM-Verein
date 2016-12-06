using System;
using System.ComponentModel;
using SettingsProviderNet;

namespace Mitgliederverwaltung.Services
{
    public class DemoSettings
    {
        [DefaultValue("Demo String")]
        public string TestString { get; set; }

        [DefaultValue("")]
        public string DefaultPath { get; set; }
    }

    internal class DemoService : IDemoService
    {
        private readonly ISettingsProvider _settingsProvider;

        public DemoService(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public string PcName => Environment.UserName;

        public string SavedText
        {
            get
            {
                var settings = _settingsProvider.GetSettings<DemoSettings>();
                return settings.TestString;
            }
            set
            {
                var settings = new DemoSettings {TestString = value};
                _settingsProvider.SaveSettings(settings);
            }
        }

        public string DefaultPath
        {
            get
            {
                var settings = _settingsProvider.GetSettings<DemoSettings>();
                return settings.DefaultPath;
            }
            set
            {
                var settings = new DemoSettings {DefaultPath = value};
                _settingsProvider.SaveSettings(settings);
            }
        }
    }
}