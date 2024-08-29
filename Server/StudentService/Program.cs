using DataLayer.Contexts;
using JwtPreset;
using Microsoft.EntityFrameworkCore;
using StudentService.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AcademyContext>(opts =>
{
    opts.UseInMemoryDatabase("InMemAcademy");
});

builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapStudentEndpoints();

app.Run();

