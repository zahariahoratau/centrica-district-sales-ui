using System;
using Caliburn.Micro;

namespace DistrictSales.UI.Presentation.ViewModels.Districts;

public class DistrictDetailsViewModel : Screen
{
    public DistrictDetailsViewModel(Guid selectedDistrictId)
    {
        var districtSalesApi = SdkHelper.GetDistrictSalesApi();
        SelectedDistrict = districtSalesApi
                .GetDistrictAsync(selectedDistrictId)
                .Result.Content?
                .MapToDistrict() ??
            throw new NullReferenceException("District not found.");
    }

    public District SelectedDistrict { get; }
}
