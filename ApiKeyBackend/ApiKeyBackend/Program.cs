using ApiKeyBackend;
using ApiKeyBackend.Controller;
using ApiKeyBackend.Models;
using ApiKeyBackend.Repository;
using ApiKeyBackend.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;


// Add services to the container.

var data = configuration.GetConnectionString("conStr");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(data));
// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


List<string> originlist = new List<string>();
var dbcontext = builder.Services.BuildServiceProvider().GetService<ApplicationDbContext>();
if (dbcontext != null)
{
    var origindata = dbcontext.Products.ToList();
    foreach (var item in origindata)
    {
        originlist.Add(item.CorsPolicy);
       originlist.Add("http://localhost:3000");
    }

};


/*var cores*/
//builder.Services.AddCustomAuthMiddleware();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "MyPolicy",
//      builder =>
//      {
//          builder.WithOrigins("http://localhost:3000")
//                                    .AllowAnyOrigin()
//                                    .AllowAnyHeader()
//                                    .AllowAnyMethod();
//      });

//});



//Cors
builder.Services.AddCors(p => p.AddDefaultPolicy(build =>
{
    build.WithOrigins(originlist.ToArray());
    //build.WithOrigins("*");
    build.AllowAnyMethod();
    build.AllowAnyHeader();
}));



var app = builder.Build();
//app.UseMiddleware<CustomAuthMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCorsMiddleware();
//app./*UseCustomAuthMiddleware*/
//app.UseCors("MyPolicy");
app.UseCors();
app.UseHttpsRedirection();
//app.UseCors("MyPolicy");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/getcors", (IAuthenticateRepository repository) =>
{
    var cors = repository.GetCors();
    if (cors != null) return cors;
    return null;
})
.WithName("getcors")
.WithOpenApi();




app.MapGroup("/minimalAPI")
   .LoginRegisterAPI()
    .WithTags("Services");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
//public static class CorsMiddlewareExtensions
//{
//    public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
//    {
//        return builder.UseMiddleware<CorsMiddleware>();
//    }
//}
//public class CorsMiddleware
//{
//    private readonly RequestDelegate _next;

//    public CorsMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public Task Invoke(HttpContext httpContext, IServiceProvider serviceProvider)
//    {
//        List<string> originlist = new List<string>();

//        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
//        var corsPolicy = dbContext.Products.ToList();
//        var ports = new List<string>();
//        foreach( var c in corsPolicy)
//        {
//            ports.Add(c.CorsPolicy);
//        }
//        StringValues datas;
//        bool datasss = false;
//        if (httpContext.Request.Headers.TryGetValue("origin", out datas))
//        {
//            datasss = ports.Contains(datas.FirstOrDefault());
//            if (datasss)
//            {
//                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", datas.FirstOrDefault());
//            }
//        }
//        httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
//        httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name");
//        httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
//        return _next(httpContext);

//    }
//}