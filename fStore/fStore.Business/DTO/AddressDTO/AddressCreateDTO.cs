namespace fStore.Business;

public class AddressCreateDTO
{
    public string HouseNumber { get; set; }
    public string Street { get; set; }
    public string PostCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public Guid? UserId { get; set; }
}

