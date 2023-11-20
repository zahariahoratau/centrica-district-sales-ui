using System.Linq;

namespace DistrictSales.UI.Presentation.Mapping;

public static class DtoToDomainMapping
{
    public static Salesperson MapToSalesperson(this SalespersonResponseV1 salespersonResponseV1)
    {
        return new Salesperson
        {
            Id = salespersonResponseV1.Id,
            FirstName = salespersonResponseV1.FirstName,
            LastName = salespersonResponseV1.LastName,
            BirthDate = salespersonResponseV1.BirthDate,
            HireDate = salespersonResponseV1.HireDate,
            Email = salespersonResponseV1.Email,
            PhoneNumber = salespersonResponseV1.PhoneNumber
        };
    }

    public static District MapToDistrict(this DistrictResponseV1 districtResponseV1)
    {
        return new District
        {
            Id = districtResponseV1.Id,
            Name = districtResponseV1.Name,
            CreatedAtUtc = districtResponseV1.CreatedAtUtc,
            IsActive = districtResponseV1.IsActive,
            NumberOfStores = districtResponseV1.NumberOfStores,
            PrimarySalesperson = districtResponseV1.PrimarySalesperson.MapToSalesperson(),
            SecondarySalespeople = districtResponseV1.SecondarySalespeople.Select(salesperson => salesperson.MapToSalesperson()).ToList()
        };
    }
}
