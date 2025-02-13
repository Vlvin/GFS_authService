

using System.Globalization;

namespace Backend.Helpers;

public class JwtConfiguration
{
    public string Issuer { get; } = string.Empty;

    public string Secret { get; } = string.Empty;

    public string Audience { get; } = string.Empty;

    public int ExpireDays { get; }

    public JwtConfiguration(IConfiguration configuration)
    {
        var section = configuration.GetSection("JWT");

        Issuer = section[nameof(Issuer)] ?? throw new Exception("You should provide Issuer in Configuration");
        Secret = section[nameof(Secret)] ?? throw new Exception("You should provide Secret in Configuration");
        Audience = section[nameof(Audience)] ?? throw new Exception("You should provide Audience in Configuration");
        ExpireDays = Convert.ToInt32(section[nameof(ExpireDays)], CultureInfo.InvariantCulture);
    }
}