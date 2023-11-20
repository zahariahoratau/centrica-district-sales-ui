using System;
using Caliburn.Micro;
using DistrictSales.UI.Domain.Models;
using DistrictSales.UI.Presentation.Helpers;
using DistrictSales.UI.Presentation.Mapping;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictDetailsViewModel : Screen
{
    public DistrictDetailsViewModel(Guid selectedDistrictId)
    {
        var districtSalesApi = RestServiceFetcher.GetDistrictSalesApi();
        SelectedDistrict = districtSalesApi
                .GetDistrictAsync(selectedDistrictId)
                .Result.Content?
                .MapToDistrict() ??
            throw new NullReferenceException("District not found.");
    }

    public District SelectedDistrict { get; }
}
