using System;
using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using DistrictSales.UI.Domain.Models;
using DistrictSales.UI.Presentation.Helpers;
using DistrictSales.UI.Presentation.Mapping;
using DistrictSales.UI.Presentation.Sdk;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictRemoveSecondarySalespersonViewModel : Screen
{
    private readonly IDistrictSalesApi _districtSalesApi;

    public DistrictRemoveSecondarySalespersonViewModel(Guid selectedDistrictId)
    {
        _districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();

        _selectedDistrict = _districtSalesApi
                .GetDistrictAsync(selectedDistrictId)
                .Result.Content?
                .MapToDistrict() ??
            throw new InvalidOperationException($"District with ID {selectedDistrictId} not found");

        _selectedDistrictSecondarySalespeople = new ObservableCollection<Salesperson>(_selectedDistrict.SecondarySalespeople);
    }

    private District _selectedDistrict;

    public District SelectedDistrict
    {
        get => _selectedDistrict;
        set
        {
            _selectedDistrict = value;
            NotifyOfPropertyChange(() => SelectedDistrict);
        }
    }

    private ObservableCollection<Salesperson> _selectedDistrictSecondarySalespeople;

    public ObservableCollection<Salesperson> SelectedDistrictSecondarySalespeople
    {
        get => _selectedDistrictSecondarySalespeople;
        set
        {
            _selectedDistrictSecondarySalespeople = value;
            NotifyOfPropertyChange(() => SelectedDistrictSecondarySalespeople);
        }
    }

    private Salesperson? _selectedSecondarySalesperson;

    public Salesperson? SelectedSecondarySalesperson
    {
        get => _selectedSecondarySalesperson;
        set
        {
            _selectedSecondarySalesperson = value;
            NotifyOfPropertyChange(() => SelectedSecondarySalesperson);
        }
    }

    public void RemoveSecondarySalespersonCommand()
    {
        if (SelectedSecondarySalesperson is null)
            return;

        string salespersonName = $"{SelectedSecondarySalesperson.FirstName} {SelectedSecondarySalesperson.LastName}";
        MessageBoxResult result = MessageBox.Show(
            $"Are you sure you want to remove {salespersonName} from being a secondary salesperson?",
            "Remove Secondary Salesperson",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );

        if (result is not MessageBoxResult.Yes)
            return;

        ApiResponse<object> apiResponse = _districtSalesApi
            .RemoveSecondarySalespersonAsync(SelectedDistrict.Id, SelectedSecondarySalesperson.Id)
            .Result;

        if (!apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Failed to remove secondary salesperson from district",
                "Remove Secondary Salesperson",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }

        SelectedDistrictSecondarySalespeople.Remove(SelectedSecondarySalesperson);
    }

    public void CancelCommand()
    {
        TryClose();
    }
}
