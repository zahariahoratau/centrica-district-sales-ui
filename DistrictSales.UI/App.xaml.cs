using System.Windows;
using DistrictSales.UI.Presentation;

namespace DistrictSales.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Configure Caliburn.Micro bootstrapper
        var bootstrapper = new Bootstrapper();

        // Continue with the default WPF startup
        base.OnStartup(e);
    }
}
