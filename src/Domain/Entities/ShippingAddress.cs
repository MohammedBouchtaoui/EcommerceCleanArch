namespace Domain.Entities;

public class ShippingAddress
{
    public string FullName { get; set; } = string.Empty;
    public string Line1 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
