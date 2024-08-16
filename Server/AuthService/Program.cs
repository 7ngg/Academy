using AuthService.Data;
using AuthService.Extensions;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Repositories;
using AuthService.Interfaces;
using AuthService.Services;
using AuthService.Validators;
using DataLayer.Contexts;
using FluentValidation;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddValidatorsFromAssemblyContaining<SignUpValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SignInValidator>();

builder.Services.AddDbContext<AcademyContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
