using System;
using DistrictSales.Api.Dto.V1.Requests;
using DistrictSales.UI.Presentation.ViewModels.Districts;
using DistrictSales.UI.Presentation.ViewModels.Salespeople;

namespace DistrictSales.UI.Presentation.Mapping;

public static class ViewModelToDtoMapping
{
    public static UpdateDistrictRequestV1 MapToUpdateDistrictRequest(this DistrictUpdateViewModel districtUpdateViewModel)
    {
        return new UpdateDistrictRequestV1(
            name: districtUpdateViewModel.Name,
            primarySalespersonId: districtUpdateViewModel.PrimarySalesperson.Id,
            isActive: districtUpdateViewModel.IsActive,
            numberOfStores: districtUpdateViewModel.NumberOfStores
        );
    }

    public static CreateDistrictRequestV1 MapToCreateDistrictRequest(this DistrictCreateViewModel districtCreateViewModel)
    {
        return new CreateDistrictRequestV1(
            name: districtCreateViewModel.Name,
            primarySalespersonId: districtCreateViewModel.PrimarySalesperson.Id,
            isActive: districtCreateViewModel.IsActive,
            numberOfStores: districtCreateViewModel.NumberOfStores
        );
    }

    public static UpdateSalespersonRequestV1 MapToUpdateSalespersonRequest(this SalespersonUpdateViewModel salespersonUpdateViewModel)
    {
        return new UpdateSalespersonRequestV1(
            firstName: salespersonUpdateViewModel.FirstName,
            lastName: salespersonUpdateViewModel.LastName,
            birthDate: salespersonUpdateViewModel.BirthDate is not null ? DateOnly.FromDateTime(salespersonUpdateViewModel.BirthDate.Value) : null,
            hireDate: salespersonUpdateViewModel.HireDate is not null ? DateOnly.FromDateTime(salespersonUpdateViewModel.HireDate.Value) : null,
            email: salespersonUpdateViewModel.Email,
            phoneNumber: salespersonUpdateViewModel.PhoneNumber
        );
    }

    public static CreateSalespersonRequestV1 MapToCreateSalespersonRequest(this SalespersonCreateViewModel salespersonCreateViewModel)
    {
        return new CreateSalespersonRequestV1(
            firstName: salespersonCreateViewModel.FirstName,
            lastName: salespersonCreateViewModel.LastName,
            birthDate: DateOnly.FromDateTime(salespersonCreateViewModel.BirthDate),
            hireDate: DateOnly.FromDateTime(salespersonCreateViewModel.HireDate),
            email: salespersonCreateViewModel.Email,
            phoneNumber: salespersonCreateViewModel.PhoneNumber
        );
    }
}
