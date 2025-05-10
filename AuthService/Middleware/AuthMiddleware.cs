using AuthService.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(
            RequestDelegate next,
            ILogger<AuthMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<IAuthorizationService>();
            var userService = context.RequestServices.GetRequiredService<UserService>();
            if (context.User.Identity.IsAuthenticated)
            {
                var id = Guid.Parse(context.User.Identity.Name);
                var user = userService.GetUser(id);
                _logger.LogInformation($"Acepted request from User:{user.Id} Login: {user.Login} Is admin: {user.Admin}");

                if (user.RevokedOn != null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }
    }
}
