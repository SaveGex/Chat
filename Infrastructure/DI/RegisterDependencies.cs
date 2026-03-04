
using Application.Options;
using Domain.Interfaces;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    public static class RegisterDependencies
    {
        public static void AddDBDependencies(this IServiceCollection services)
        {
            services.AddDbContext<SchoolChatContext>();

            services.AddScoped<IChatsCRUDRepository, ChatsCRUDRepository>();
            services.AddScoped<IChatsQueryRepository, ChatsQueryRepository>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();

            services.AddSingleton<JwtOptions>();
            services.AddSingleton<ITokenCacheService, TokenCacheService>();
            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
