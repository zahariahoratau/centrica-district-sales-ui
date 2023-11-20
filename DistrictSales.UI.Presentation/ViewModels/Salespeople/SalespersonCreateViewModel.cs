using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using DistrictSales.Api.Dto.V1.Requests;
using DistrictSales.Api.Dto.V1.Responses;
using DistrictSales.UI.Presentation.Helpers;
using DistrictSales.UI.Presentation.Mapping;
using DistrictSales.UI.Presentation.Sdk;
using Refit;

namespace DistrictSales.UI.Presentation.ViewModels.Salespeople;


public class SalespersonCreateViewModel : Screen, IDataErrorInfo
{
    private readonly IDistrictSalesApi _districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();

    private string _firstName;
    private string _lastName;
    private DateTime _birthDate = DateTime.Now;
    private DateTime _hireDate = DateTime.Now;
    private string _email;
    private string? _phoneNumber;

    // Bindable properties
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            NotifyOfPropertyChange(() => FirstName);
            NotifyOfPropertyChange(() => CanCreateSalesperson);
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            NotifyOfPropertyChange(() => LastName);
            NotifyOfPropertyChange(() => CanCreateSalesperson);
        }
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set
        {
            _birthDate = value;
            NotifyOfPropertyChange(() => BirthDate);
            NotifyOfPropertyChange(() => CanCreateSalesperson);
        }
    }

    public DateTime HireDate
    {
        get => _hireDate;
        set
        {
            _hireDate = value;
            NotifyOfPropertyChange(() => HireDate);
            NotifyOfPropertyChange(() => CanCreateSalesperson);
        }
    }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Email Address is Invalid")]
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            NotifyOfPropertyChange(() => Email);
            NotifyOfPropertyChange(() => CanCreateSalesperson);
        }
    }

    public string? PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            NotifyOfPropertyChange(() => PhoneNumber);
            NotifyOfPropertyChange(() => CanCreateSalesperson);
        }
    }

    public bool CanCreateSalesperson
    {
        get
        {
            // Check if all the required fields are filled
            return !string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(Email) &&
                BirthDate != default &&
                HireDate != default;
        }
    }

    // IDataErrorInfo implementation for validation
    public string this[string columnName]
    {
        get
        {
            switch (columnName)
            {
                case nameof(FirstName):
                    return string.IsNullOrWhiteSpace(FirstName) ? "First Name is required." : null;

                case nameof(LastName):
                    return string.IsNullOrWhiteSpace(LastName) ? "Last Name is required." : null;

                case nameof(Email):
                    return string.IsNullOrWhiteSpace(Email) ? "Email is required." : null;

                case nameof(BirthDate):
                    return BirthDate == default ? "Birth Date is required." : null;

                case nameof(HireDate):
                    return HireDate == default ? "Hire Date is required." : null;

                default:
                    return null;
            }
        }
    }

    public string Error => null;

    public void CreateSalespersonCommand()
    {
        ApiResponse<SalespersonResponseV1> apiResponse = _districtSalesApi
            .CreateSalespersonAsync(this.MapToCreateSalespersonRequest())
            .Result;

        if (apiResponse.IsSuccessStatusCode)
        {
            MessageBox.Show("Salesperson created!");
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
