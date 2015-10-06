using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;
using System.Data.SqlClient;
using System.Security;

namespace AdministrationProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string programNamen = "AdministrationProgram";
        public const string logName = programNamen + "Logg";

        public static new App Current { get { return Application.Current as App; } }
        internal Settings settings { get; private set; }

        string settingsFile;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            settingsFile = System.IO.Path.Combine(System.Environment.CurrentDirectory, "settings.bin");

            settings = Settings.load(settingsFile);
            if (settings == null)
               settings = new Settings();

            bool deploy = false;

            foreach (var item in e.Args)
            {
                if (item == "deploy")
                    deploy = true;
            }

            if (settings.firstStart || deploy)
                new wpf.ConfigurationWizard.Wizard(
                    settings.clone(), SaveSettings).Show();
            else
                new wpf.MainWindow(settings).Show();
        }

        void SaveSettings(object s, EventArgs e)
        {
            wpf.ConfigurationWizard.Wizard w = s as wpf.ConfigurationWizard.Wizard;
            w.settings.firstStart = false;
            Settings.Save(settingsFile, w.settings);
            settings = w.settings;
            new wpf.MainWindow(settings).Show();
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
            throw new NotImplementedException();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //throw new NotImplementedException();
        }

    }
}
