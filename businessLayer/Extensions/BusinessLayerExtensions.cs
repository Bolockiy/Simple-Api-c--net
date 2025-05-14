using ApiToDo.Domain.Entities;
using BusinessLayer.BackService;
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
        services.AddHostedService<Service1>();
        services.AddHostedService<Service2>();
        return services;
    }
}
