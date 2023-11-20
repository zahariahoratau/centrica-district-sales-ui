using System.Configuration;
using Refit;

namespace DistrictSales.UI.Presentation.Helpers;

public static class SdkHelper
{
    public static IDistrictSalesApi GetDistrictSalesApi()
    {
        string? apiHost = ConfigurationManager.AppSettings.Get("DistrictSalesApiHost");

        if (apiHost is null)
            throw new ConfigurationErrorsException("DistrictSalesApiHost is not configured in the App.config file.");

        return RestService.For<IDistrictSalesApi>(apiHost);
    }
}
