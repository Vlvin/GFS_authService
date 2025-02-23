namespace Backend.Models;
public record AuthorizeResponse(bool authorized, string UserName, string Email);
