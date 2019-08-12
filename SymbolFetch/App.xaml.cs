using System.Windows;

namespace SymbolFetch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ResourceDownloader.WriteToLog("Unhandled exception", e.Exception);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 2)
            {
                Shutdown();
                return;
            }
            new MainWindow(e.Args[0], e.Args[1].Split('|')).Show();
        }
    }
}
