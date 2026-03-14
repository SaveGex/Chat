using Microsoft.EntityFrameworkCore;

namespace ChatApi.Extentions
{
    public static class WebApplicationExtentions
    {
        public static IApplicationBuilder ExecuteMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Infrastructure.DB.SchoolChatContext>();
            db.Database.Migrate();
            return app;
        }
    }
}
