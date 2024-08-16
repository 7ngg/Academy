using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtPreset;

public static class JwtPreset
{
    public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration.GetSection("JwtOptions:SecretKey").Value;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
                };

                opts.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(opts =>
        {
            opts.AddPolicy("ADMIN", policy => policy.RequireRole("ADMIN"));
            opts.AddPolicy("TEACHER", policy => policy.RequireRole("TEACHER"));
            opts.AddPolicy("STUDENT", policy => policy.RequireRole("STUDENT"));
        });
    }
}
