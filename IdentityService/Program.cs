using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using IdentityService;
using IdentityService.Contracts;
using IdentityService.Service;
using IdentityService.Repositories;
using IdentityService.Provider;
using IdentityService.Mapper;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Конфигурация JWT 
builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

// База данных и AutoMapper
builder.Services.AddDb(configuration);
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddControllers();

//Аутентификация
builder.Services.AddApiAuthentication(configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

///Сервисы аутентификации
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
var app = builder.Build();

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

// ✅ МАППИНГ КОНТРОЛЛЕРОВ
app.MapControllers();

app.Run();