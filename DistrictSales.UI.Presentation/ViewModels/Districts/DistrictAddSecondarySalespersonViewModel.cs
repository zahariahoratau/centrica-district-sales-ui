using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictAddSecondarySalespersonViewModel : Screen
{
    private readonly IDistrictSalesApi _districtSalesApi;

    public DistrictAddSecondarySalespersonViewModel(Guid selectedDistrictId)
    {
        _districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();

        _selectedDistrict = _districtSalesApi
                .GetDistrictAsync(selectedDistrictId)
                .Result.Content?
                .MapToDistrict() ??
            throw new InvalidOperationException($"District with ID {selectedDistrictId} not found");
        _selectedDistrictSecondarySalespeople = new ObservableCollection<Salesperson>(_selectedDistrict.SecondarySalespeople);

        _possibleSecondarySalespeople = new BindableCollection<Salesperson>(
            _districtSalesApi
                .GetSalespeopleAsync()
                .Result.Content?
                .Select(salesperson => salesperson.MapToSalesperson()) ??
            new List<Salesperson>()
        );
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

    private BindableCollection<Salesperson> _possibleSecondarySalespeople;

    public BindableCollection<Salesperson> PossibleSecondarySalespeople
    {
        get => _possibleSecondarySalespeople;
        set
        {
            _possibleSecondarySalespeople = value;
            NotifyOfPropertyChange(() => PossibleSecondarySalespeople);
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

    public void AddSecondarySalespersonCommand()
    {
        if (SelectedSecondarySalesperson is null)
            return;

        IApiResponse apiResponse = _districtSalesApi
            .AddSecondarySalespersonAsync(SelectedDistrict.Id, SelectedSecondarySalesperson.Id)
            .Result;

        if (!apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show(
                "Failed to add secondary salesperson",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }

        SelectedDistrictSecondarySalespeople.Add(SelectedSecondarySalesperson);
    }

    public void CancelCommand()
    {
        TryClose();
    }
}
