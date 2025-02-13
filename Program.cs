using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Backend.Helpers;
using Backend.Models;
using Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<JwtConfiguration>();
builder.Services.AddTransient<JwtTokenProvider>();
builder.Services.AddTransient<AuthUser>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };


    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});
builder.Services
  .AddAuthentication(options =>
  {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  }).AddJwtBearer(options =>
  {
      options.SaveToken = true;
      options.TokenValidationParameters = new()
      {
          ValidateAudience = false,
          ValidateIssuer = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey
          (
            Encoding.UTF8.GetBytes
            (
                config.GetSection("JWT")["Secret"] ?? throw new Exception("You must configure the JWT:Secret"))),
      };
      options.Events = new JwtBearerEvents
      {
          OnMessageReceived = context =>
          {
              string? authorization = context.Request.Headers["Authorization"];
              if (string.IsNullOrEmpty(authorization))
              {
                  context.NoResult();
              }
              else
              {
                  context.Token = authorization.Replace("Bearer ", string.Empty);
              }
              return Task.CompletedTask;
          },
      };
  });
builder.Services.AddAuthorization();

var connectionString = config.GetConnectionString("Default");
builder.Services.AddDbContext<Context>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<AuthUser, IdentityRole>(options => {
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
  }).AddEntityFrameworkStores<Context>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().WithOpenApi();
app.Run();
