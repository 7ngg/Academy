using DataLayer.Contexts;
using DepartmentService.Endpoints;
using DepartmentService.Repositories;
using DepartmentService.Repositories.Interfaces;
using DepartmentService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AcademyContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService.Services.DepartmentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDepartmentEnpoints();

app.Run();
