using Application.Orchestrations;
using Application.Orchestrations.Interfaces;
using Application.Services;
using Application.Services.Interfaces;
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
            services.AddScoped<IUsersService, UsersService>();

            services.AddSingleton<IChatsAccessOrchestrator, ChatsAccessOrchestrator>();
        }
    }
}
