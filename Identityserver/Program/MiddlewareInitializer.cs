using AutoWrapper;

namespace Identityserver.Program;

public static partial class MiddlewareInitializer
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {

        app.UseRouting();
        app.UseCors();
        app.UseIdentityServer();
        ConfigureSwagger(app);
        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseAutoWrapper();
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
    }

    private static void ConfigureIdentity(WebApplication app)
    {
        
    }
}