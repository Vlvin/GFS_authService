using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class AuthUser : IdentityUser
{

    [Key]
    [Required]
    override required public string Email { get; set; }

    [Required]
    override required public string UserName { get; set; }
}
