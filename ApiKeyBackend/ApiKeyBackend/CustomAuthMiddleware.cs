using ApiKeyBackend;
using ApiKeyBackend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace ApiKeyBackend
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public  Task InvokeAsync(HttpContext httpContext, IServiceProvider serviceProvider)
        {
            //var url = context.Request.Path;

            //if (url== "/minimalAPI/users")// Check if the request is for the "Users" endpoint
            //{
            //    List<string> originlist = new List<string>();

            //    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            //    var corsPolicy = dbContext.Products.ToList();
            //    foreach (var item in corsPolicy)
            //    {
            //        originlist.Add(item.CorsPolicy);
            //        //originlist.Add("http://localhost:3001");
            //    }
            //    if (corsPolicy != null)
            //    {
            //        // Set CORS policy based on data from the database
            //        context.Response.Headers.Add("Access-Control-Allow-Origin", originlist.ToArray());
            //        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            //        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, abcdefgh");
            //    }
            //}
            //else
            //{
            //    // Allow all CORS origins for other endpoints
            //    context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3001");
            //    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
            //    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, ApiKey");
            //}

            //await _next(context);



            //Sur

            List<string> originlist = new List<string>();

            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var corsPolicy = dbContext.Products.ToList();
            var ports = new List<string>();
            foreach (var c in corsPolicy)
            {
                ports.Add(c.CorsPolicy);
            }
            StringValues datas;
            bool datasss = false;
            if (httpContext.Request.Headers.TryGetValue("origin", out datas))
            {
                datasss = ports.Contains(datas.FirstOrDefault());
                if (datasss)
                {
                    httpContext.Response.Headers.Add("Access-Control-Allow-Origin", datas.FirstOrDefault());
                }
            }
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            return _next(httpContext);
        }
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class CustomAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder)
    {

        return builder.UseMiddleware<CustomAuthMiddleware>();
    }
}
