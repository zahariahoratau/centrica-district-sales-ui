using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using DistrictSales.UI.Domain.Models;
using DistrictSales.UI.Presentation.Helpers;
using DistrictSales.UI.Presentation.Mapping;
using DistrictSales.UI.Presentation.Sdk;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Salespeople;

public class SalespeopleMenuViewModel : Conductor<Screen>.Collection.OneActive
{
    private readonly IWindowManager _windowManager;
    private readonly IDistrictSalesApi _districtSalesApi;

    public SalespeopleMenuViewModel(IWindowManager windowManager)
    {
        _windowManager = windowManager;

        _districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();
        _salespeople =
            new ObservableCollection<Salesperson>(
                _districtSalesApi.GetSalespeopleAsync()
                    .Result.Content?
                    .Select(salesperson => salesperson.MapToSalesperson())
                    .ToList() ??
                new List<Salesperson>()
            );
    }

    private ObservableCollection<Salesperson> _salespeople;

    public ObservableCollection<Salesperson> Salespeople
    {
        get { return _salespeople; }
        set
        {
            _salespeople = value;
            NotifyOfPropertyChange(() => Salespeople);
        }
    }

    private Salesperson? _selectedSalesperson;

    public Salesperson? SelectedSalesperson
    {
        get => _selectedSalesperson;
        set
        {
            _selectedSalesperson = value;
            NotifyOfPropertyChange(() => SelectedSalesperson);
        }
    }

    public void ReadDetailsCommand()
    {
        if (SelectedSalesperson == null)
            return;

        var detailsViewModel = new SalespersonDetailsViewModel(SelectedSalesperson.Id);
        _windowManager.ShowWindow(detailsViewModel);
    }

    public void CreateCommand()
    {
        var createViewModel = new SalespersonCreateViewModel();
        _windowManager.ShowWindow(createViewModel);
    }

    public void UpdateCommand()
    {
        if (SelectedSalesperson == null)
            return;

        var updateViewModel = new SalespersonUpdateViewModel(SelectedSalesperson.Id);
        _windowManager.ShowWindow(updateViewModel);
    }

    public void DeleteCommand()
    {
        if (SelectedSalesperson == null)
            return;

        MessageBoxResult result = MessageBox
            .Show(
                $"Do you want to delete {SelectedSalesperson.FirstName} {SelectedSalesperson.LastName}?",
                "Delete Salesperson",
                MessageBoxButton.YesNoCancel
            );

        if (result != MessageBoxResult.Yes)
            return;

        ApiResponse<object> apiResponse = _districtSalesApi.DeleteSalespersonAsync(SelectedSalesperson.Id).Result;

        if (!apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Failed to delete salesperson.",
                "Delete Salesperson",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }

        Salespeople.Remove(SelectedSalesperson);
    }
}
