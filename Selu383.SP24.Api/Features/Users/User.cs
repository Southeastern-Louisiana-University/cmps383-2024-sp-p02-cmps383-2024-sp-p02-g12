using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Features.Hotels;
using System.ComponentModel.DataAnnotations;

namespace Selu383.SP24.Api.Features.Users
{
    public class User : IdentityUser<int>
    {
     //   public int? ManagerId { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

        public virtual ICollection<Hotel> ManageHotels { get; set; } = new List<Hotel>();

    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
    }

    public class CreateUserDto
    {
        [Required] public string UserName { get; set; } = string.Empty;

        [Required] public string Password { get; set; } = string.Empty;

        [Required] public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
