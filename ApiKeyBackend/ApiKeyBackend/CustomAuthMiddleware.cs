using ApiKeyBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

public class CustomAuthMiddleware : IEndpointFilter
{
    private readonly IServiceProvider serviceProvider;
    private readonly HttpContext httpContext;
    public CustomAuthMiddleware(IServiceProvider _serviceProvider, HttpContext httpContext)
    {
        serviceProvider = _serviceProvider;
        this.httpContext = httpContext;

    }

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {

        List<string> originlist = new List<string>();

        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var corsPolicy = dbContext.Products.ToList();
        var ports = new List<string>();
        foreach (var c in corsPolicy)
        {
            ports.Add(c.CorsPolicy);
            ports.Add("http://localhost:3000");
        }
        StringValues datas;
        bool datasss = false;
        if (context.HttpContext.Response.Headers.TryGetValue("origin", out datas))
        {
            datasss = ports.Contains(datas.FirstOrDefault());
            if (datasss)
            {
                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", datas.FirstOrDefault());
            }
        }
        //httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        //context.HttpContext.Response.Headers
        context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, ApiKey");
        context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
        return next(context);
    }
    //public static class CustomAuthMiddlewareExtensions
    //{
    //    public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder)
    //    {

    //        return builder.UseMiddleware<CustomAuthMiddleware>();
    //    }
    //}
}
