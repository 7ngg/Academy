using DataLayer.Contexts;
using Microsoft.EntityFrameworkCore;
using UserService.Endpoint;
using UserService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AcademyContext>(opts => opts.UseInMemoryDatabase("UsersInMemo"));

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.Run();

