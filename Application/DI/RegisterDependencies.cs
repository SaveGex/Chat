using Application.Orchestrations;
using Application.Orchestrations.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    public static class RegisterDependencies
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IChatsAccessService, ChatAccessService>();
            services.AddScoped<IChatsService, ChatsService>();
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IMessagesService, MessagesService>();

            services.AddSingleton<IGenericOrchestrator<IUsersRepository>, GenericOrchestrator<IUsersRepository>>();
            services.AddSingleton<IGenericOrchestrator<IChatsAccessService>, GenericOrchestrator<IChatsAccessService>>();
            services.AddSingleton<IUsersService, UsersService>();
        }
    }
}
