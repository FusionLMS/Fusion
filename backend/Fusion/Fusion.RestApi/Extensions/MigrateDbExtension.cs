using System.Diagnostics.CodeAnalysis;
using Fusion.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Fusion.RestApi.Extensions;

[ExcludeFromCodeCoverage]
public static class MigrateDbExtension
{
   /// <summary>
   /// Extension method to apply migrations to the database on application startup
   /// </summary>
   /// <param name="app"></param>
   /// <returns></returns>
   public static void ApplyMigrations(this IApplicationBuilder app)
   {
      using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

      using ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

      dbContext.Database.Migrate();
   }
}