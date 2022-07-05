using Dating_App___API.Data;
using Dating_App___API.Extensions;
using Dating_App___API.Interfaces;
using Dating_App___API.Middleware;
using Dating_App___API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/*//Build/migrates database when restaring app

using var scope = builder.Build().Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}

catch(Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}*/



// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddAplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("----ADRESS FRONT----"));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();