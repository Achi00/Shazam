using Microsoft.EntityFrameworkCore;
using Shazam.Application.Interfaces;
using Shazam.Application.Interfaces.Repository;
using Shazam.Application.Interfaces.Service.Song;
using Shazam.Application.Interfaces.Services;
using Shazam.Application.Services;
using Shazam.Application.Services.Songs;
using Shazam.Infrastructure.Redis;
using Shazam.Infrastructure.Repositories;
using Shazam.Persistence;
using Shazam.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFingerprintRepository, RedisFingerprintRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<ProcessYoutubeService>();

builder.Services.AddDbContext<ShazamContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionString.DefaultConnection))));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
