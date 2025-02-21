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

    [Required]
    public ICollection<Post> Posts { get; set; } = [];
}

public class UserDTO(AuthUser user)
{
    public string Id { get; set; } = user.Id;
    public string Email { get; set; } = user.Email;
    public string UserName { get; set; } = user.UserName;
}
