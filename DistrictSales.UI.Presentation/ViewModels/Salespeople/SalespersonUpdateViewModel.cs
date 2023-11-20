using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Salespeople;

public class SalespersonUpdateViewModel : Screen, IDataErrorInfo
{
    private readonly IDistrictSalesApi _districtSalesApi;

    public SalespersonUpdateViewModel(Guid selectedSalespersonId)
    {
        _districtSalesApi = SdkHelper.GetDistrictSalesApi();
        _selectedSalesperson = _districtSalesApi
                .GetSalespersonAsync(selectedSalespersonId)
                .Result.Content
                ?.MapToSalesperson() ??
            throw new NullReferenceException("Salesperson not found");
    }

    private readonly Salesperson _selectedSalesperson;

    private string? _firstName;
    private string? _lastName;
    private DateTime? _birthDate;
    private DateTime? _hireDate;
    private string? _email;
    private string? _phoneNumber;

    // Bindable properties
    public string? FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            NotifyOfPropertyChange(() => FirstName);
            NotifyOfPropertyChange(() => CanUpdateSalesperson);
        }
    }

    public string? LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            NotifyOfPropertyChange(() => LastName);
            NotifyOfPropertyChange(() => CanUpdateSalesperson);
        }
    }

    public DateTime? BirthDate
    {
        get => _birthDate;
        set
        {
            _birthDate = value;
            NotifyOfPropertyChange(() => BirthDate);
            NotifyOfPropertyChange(() => CanUpdateSalesperson);
        }
    }

    public DateTime? HireDate
    {
        get => _hireDate;
        set
        {
            _hireDate = value;
            NotifyOfPropertyChange(() => HireDate);
            NotifyOfPropertyChange(() => CanUpdateSalesperson);
        }
    }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Email Address is Invalid")]
    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            NotifyOfPropertyChange(() => Email);
            NotifyOfPropertyChange(() => CanUpdateSalesperson);
        }
    }

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            NotifyOfPropertyChange(() => PhoneNumber);
            NotifyOfPropertyChange(() => CanUpdateSalesperson);
        }
    }

    public bool CanUpdateSalesperson
    {
        get
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(Email) &&
                BirthDate != default &&
                HireDate != default;
        }
    }

    public string this[string columnName]
    {
        get => null;
    }

    public string Error => null;

    public void UpdateSalespersonCommand()
    {
        IApiResponse apiResponse = _districtSalesApi
            .UpdateSalespersonAsync(_selectedSalesperson.Id, this.MapToUpdateSalespersonRequest())
            .Result;

        if (apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show("Salesperson updated!");
            CancelCommand();
            return;
        }

        string errorMessage = apiResponse.StatusCode is HttpStatusCode.BadRequest
            ? "Error: Bad request. Please check the data and try again."
            : $"Error: {apiResponse.StatusCode} - {apiResponse.Error.Content}";

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
