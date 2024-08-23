using DataLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using TeacherService.Endpoints;
using TeacherService.Repositories;
using TeacherService.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AcademyContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapTeacherEndpoints();

app.Run();
