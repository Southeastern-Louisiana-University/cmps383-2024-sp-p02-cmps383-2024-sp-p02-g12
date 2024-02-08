using Selu383.SP24.Api.Features.Users;

namespace Selu383.SP24.Api.Features.Hotels;

public class Hotel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

   // public int? ManagerId {get; set;}

    public virtual User? Manager { get; set; }
}
