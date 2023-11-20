using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictCreateViewModel : Screen, IDataErrorInfo
{
    private readonly IDistrictSalesApi _districtSalesApi;

    public DistrictCreateViewModel()
    {
        _districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();
        _possibleSalespeople = new BindableCollection<Salesperson>(
            _districtSalesApi
                .GetSalespeopleAsync()
                .Result.Content?
                .Select(salesperson => salesperson.MapToSalesperson()) ??
            new List<Salesperson>()
        );
    }

    private string _name;
    private bool _isActive = true;
    private short? _numberOfStores;
    private Salesperson _primarySalesperson;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NotifyOfPropertyChange(() => Name);
        }
    }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            NotifyOfPropertyChange(() => IsActive);
        }
    }

    public short? NumberOfStores
    {
        get => _numberOfStores;
        set
        {
            _numberOfStores = value;
            NotifyOfPropertyChange(() => NumberOfStores);
        }
    }

    public Salesperson PrimarySalesperson
    {
        get => _primarySalesperson;
        set
        {
            _primarySalesperson = value;
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

    public void CreateDistrictCommand()
    {
        ApiResponse<DistrictResponseV1> apiResponse = _districtSalesApi
            .CreateDistrictAsync(this.MapToCreateDistrictRequest())
            .Result;

        if (apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show("District created!");
            CancelCommand();
            return;
        }

        string errorMessage = apiResponse.StatusCode == HttpStatusCode.BadRequest
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
        // Close the window or navigate back
        TryClose();
    }
}
