using System;
using Caliburn.Micro;

namespace DistrictSales.UI.Presentation.ViewModels.Salespeople;

public class SalespersonDetailsViewModel : Screen
{
    public SalespersonDetailsViewModel(Guid selectedSalespersonId)
    {
        var districtSalesApi = SdkHelper.GetDistrictSalesApi();
        SelectedSalesperson = districtSalesApi
                .GetSalespersonAsync(selectedSalespersonId)
                .Result.Content?
                .MapToSalesperson() ??
            throw new NullReferenceException("Salesperson not found.");
    }

    public Salesperson SelectedSalesperson { get; }
}
