using DataLayer.Contexts;
using FacultyService.Endpoints;
using FacultyService.Repositories;
using FacultyService.Repositories.Interfaces;
using FacultyService.Services;
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();

builder.Services.AddScoped<IFacultyService, FacultyService.Services.FacultyService>();

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

