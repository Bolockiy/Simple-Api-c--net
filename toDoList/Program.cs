
using ApiToDo.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using System.Text;
using toDoList.Entities.UserAccount;
using Helper.Security;
using toDoList.Services;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
logger.Info("Приложение запускается...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddAppServices(builder.Configuration);
    builder.Services.AddBusinessLayer();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        DbInitializer.SeedAdminUser(dbContext);
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Произошла ошибка при запуске приложения");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
