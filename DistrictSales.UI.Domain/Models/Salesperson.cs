namespace DistrictSales.UI.Domain.Models;

public class Salesperson
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}
