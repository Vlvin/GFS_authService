using Backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;
public class Context(DbContextOptions<Context> options) : IdentityDbContext<AuthUser>(options)
{
}