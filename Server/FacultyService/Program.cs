using DataLayer.Contexts;
using FacultyService.Endpoints;
using FacultyService.Repositories;
using FacultyService.Repositories.Interfaces;
using JwtPreset;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddDbContext<AcademyContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapFacultyEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

