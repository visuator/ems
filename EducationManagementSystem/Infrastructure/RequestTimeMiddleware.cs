namespace EducationManagementSystem.Infrastructure;

public class RequestTimeMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //context.Items["requestedAt"] = DateTime.UtcNow;
        return next.Invoke(context);
    }
}