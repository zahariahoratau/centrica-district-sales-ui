using System;
using Caliburn.Micro;
using DistrictSales.UI.Domain.Models;
using DistrictSales.UI.Presentation.Helpers;
using DistrictSales.UI.Presentation.Mapping;

namespace DistrictSales.UI.Presentation.ViewModels.Salespeople;

public class SalespersonDetailsViewModel : Screen
{
    public SalespersonDetailsViewModel(Guid selectedSalespersonId)
    {
        var districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();
        SelectedSalesperson = districtSalesApi
                .GetSalespersonAsync(selectedSalespersonId)
                .Result.Content?
                .MapToSalesperson() ??
            throw new NullReferenceException("Salesperson not found.");
    }

    public Salesperson SelectedSalesperson { get; }
}
