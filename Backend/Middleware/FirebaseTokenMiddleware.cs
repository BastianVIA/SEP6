using FirebaseAdmin.Auth;

namespace Backend.Middleware;

public class FirebaseTokenMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string authHeader = context.Request.Headers["Authorization"];
                string firebaseToken = authHeader.Substring("Bearer ".Length);
                FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(firebaseToken);
                string uid = decodedToken.Uid;
                context.Items[HttpContextKeys.UserId] = uid;
            }
        }
        catch (Exception e)
        {
            NLog.LogManager.GetCurrentClassLogger().Error(e.StackTrace);
            throw new UnauthorizedAccessException("Access denied");
        }
        
        await _next(context);
    }
}