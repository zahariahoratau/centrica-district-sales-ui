namespace DistrictSales.UI.Domain.Models;

public class District
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool IsActive { get; set; }
    public short? NumberOfStores { get; set; }
    public Salesperson PrimarySalesperson { get; set; }
    public List<Salesperson> SecondarySalespeople { get; set; }
}
