

namespace Backend.Models;
public record RegisterRequest(string Username, string Email, string Password);
public record RegisterResponse(string Token);