using System.Windows;
using DueskWPF.Services;

namespace DueskWPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        // Load saved session from disk
        SessionManager.Instance.LoadSession();
        
        // Handle unhandled exceptions
        DispatcherUnhandledException += App_DispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }
    
    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"Ein Fehler ist aufgetreten:\n{e.Exception.Message}", 
            "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }
    
    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            MessageBox.Show($"Ein schwerwiegender Fehler ist aufgetreten:\n{ex.Message}", 
                "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        MessageBox.Show($"Ein Hintergrund-Fehler ist aufgetreten:\n{e.Exception.Message}", 
            "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        e.SetObserved();
    }
}