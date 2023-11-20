using Caliburn.Micro;
using DistrictSales.UI.Presentation.ViewModels.Districts;
using DistrictSales.UI.Presentation.ViewModels.Salespeople;
using DistrictSales.UI.Presentation.Views.Districts;
using DistrictSales.UI.Presentation.Views.Salespeople;

namespace DistrictSales.UI.Presentation.ViewModels;

public sealed class MainViewModel : Conductor<object>
{
    private readonly IWindowManager _windowManager;

    public MainViewModel(IWindowManager windowManager)
    {
        DisplayName = "District Sales";
        _windowManager = windowManager;
    }

    public void LoadDistrictsPage()
    {
        var districtsViewModel = new DistrictsMenuViewModel(_windowManager);
        var districtsView = new DistrictsMenuView
        {
            DataContext = districtsViewModel
        };
        districtsView.Show();
    }

    public void LoadSalespeoplePage()
    {
        var salespeopleViewModel = new SalespeopleMenuViewModel(_windowManager);
        var salespeopleView = new SalespeopleMenuView
        {
            DataContext = salespeopleViewModel
        };
        salespeopleView.Show();
    }
}
