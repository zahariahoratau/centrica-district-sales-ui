using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictUpdateViewModel : Screen, IDataErrorInfo
{
    private readonly IDistrictSalesApi _districtSalesApi;

    public DistrictUpdateViewModel(Guid selectedDistrictId)
    {
        _districtSalesApi = SdkHelper.GetDistrictSalesApi();

        _selectedDistrict = _districtSalesApi
                .GetDistrictAsync(selectedDistrictId)
                .Result.Content?
                .MapToDistrict() ??
            throw new InvalidOperationException($"District with ID {selectedDistrictId} not found");

        _possibleSalespeople = new BindableCollection<Salesperson>(
            _districtSalesApi
                .GetSalespeopleAsync()
                .Result.Content?
                .Select(salesperson => salesperson.MapToSalesperson()) ??
            new List<Salesperson>()
        );
    }

    private readonly District _selectedDistrict;

    public string Name
    {
        get => _selectedDistrict.Name;
        set
        {
            _selectedDistrict.Name = value;
            NotifyOfPropertyChange(() => Name);
        }
    }

    public bool IsActive
    {
        get => _selectedDistrict.IsActive;
        set
        {
            _selectedDistrict.IsActive = value;
            NotifyOfPropertyChange(() => IsActive);
        }
    }

    public short? NumberOfStores
    {
        get => _selectedDistrict.NumberOfStores;
        set
        {
            _selectedDistrict.NumberOfStores = value;
            NotifyOfPropertyChange(() => NumberOfStores);
        }
    }

    public Salesperson PrimarySalesperson
    {
        get => _selectedDistrict.PrimarySalesperson;
        set
        {
            _selectedDistrict.PrimarySalesperson = value;
            NotifyOfPropertyChange(() => PrimarySalesperson);
        }
    }

    private BindableCollection<Salesperson> _possibleSalespeople;

    public BindableCollection<Salesperson> PossibleSalespeople
    {
        get => _possibleSalespeople;
        set
        {
            _possibleSalespeople = value;
            NotifyOfPropertyChange(() => PossibleSalespeople);
        }
    }

    public string Error { get; }

    public string this[string columnName]
    {
        get
        {
            string? error = columnName switch
            {
                nameof(Name) => string.IsNullOrWhiteSpace(Name) ? "Name is required" : null,
                nameof(IsActive) => null,
                nameof(NumberOfStores) => NumberOfStores < 0 ? "Number of stores must be greater than or equal to 0" : null,
                nameof(PrimarySalesperson) => PrimarySalesperson == null ? "Primary salesperson is required" : null,
                _ => null
            };

            return error ?? string.Empty;
        }
    }

    public void UpdateDistrictCommand()
    {
        IApiResponse apiResponse = _districtSalesApi
            .UpdateDistrictAsync(_selectedDistrict.Id, this.MapToUpdateDistrictRequest())
            .Result;

        if (apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show("District updated!");
            CancelCommand();
            return;
        }

        string errorMessage = apiResponse.StatusCode is HttpStatusCode.BadRequest
            ? "Error: Bad request. Please check the data and try again."
            : $"Error: {apiResponse.StatusCode} - {apiResponse.ReasonPhrase}";

        MessageBox.Show(
            errorMessage,
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        );
    }

    public void CancelCommand()
    {
        TryClose();
    }
}
