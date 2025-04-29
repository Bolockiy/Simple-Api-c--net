using ApiToDo.Domain.Entities;
using BusinessLayer.Model;
using BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using toDoList.Entities.UserAccount;

public static class BusinessLayerExtensions
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddScoped<ICrudService<UserAccount>, UserService>();
        services.AddScoped<ICrudService<ToDoTask>, TaskService>();
        return services;
    }
}
