using ApiKeyBackend.Models;
using ApiKeyBackend.Repository.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;

namespace ApiKeyBackend.Controller
{
    public static class Presentation
    {

        public static RouteGroupBuilder LoginRegisterAPI(this RouteGroupBuilder app)
        {
            app.MapPost("/register", Register);

            app.MapPost("/login", Login);

            app.MapGet("/users", Users);
            app.MapPost("/ValidateApi", ValidateApi);
            app.MapPost($"/addproduct", AddProduct);
            app.MapGet($"/Disable", Disable);

            return app;
            
        }
        public async static Task<IResult> Register(IAuthenticateRepository _service, [FromBody] RegisterModel register)
        {
            var user = new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email,
                PasswordHash = register.Password,
            };


            var registerUser = await _service.RegisterUser(user);
            if (!registerUser) return Results.StatusCode(StatusCodes.Status500InternalServerError);
            return Results.Ok(new { Message = "Register successfully!!!" });
        }

        public async static Task<IResult> Login(IAuthenticateRepository _service, LoginModel login)
        {
            var loginUser = await _service.AuthenticateUser(login);
            if (loginUser == null) return Results.StatusCode(StatusCodes.Status500InternalServerError);
            return Results.Ok(loginUser);

        }

        public async static Task<IResult> Users(IAuthenticateRepository _service, string id,
            IProductRepository _productRepository, HttpContext context, IHttpContextAccessor httpContextAccessor)
        {
            var user = _service.GetUsers(id);
            if (user == null) return Results.StatusCode(StatusCodes.Status404NotFound);

            var validateResult = await ValidateApi(_service, context.Request, httpContextAccessor, _productRepository, context);

            if (validateResult != null)
            {
                return Results.Ok(user);
               
            }

           return Results.StatusCode(StatusCodes.Status404NotFound);
        }

         [DisableCors]
        public async static Task<IResult> Disable(IAuthenticateRepository repository)
        {
            var cors = repository.GetCors();
            if (cors != null)
            {
                return null;
            }
            return null;
        }

        //public async static Task<IResult> ValidateApi(IAuthenticateRepository _service, HttpRequest req, IHttpContextAccessor httpContextAccessor, IProductRepository _productRepository, HttpContext context)
        //{
        //    req.Headers.TryGetValue("ApiKey", out var ApiKey);

        //    if (ApiKey == "") return Results.StatusCode(StatusCodes.Status404NotFound);
        //    var validate = _productRepository.Purchase(context, ApiKey);
        //    if (!validate) return Results.StatusCode(StatusCodes.Status404NotFound);
        //    return Results.Ok(validate);

        // }

        public async static Task<IResult> ValidateApi(IAuthenticateRepository _service, HttpRequest req, IHttpContextAccessor httpContextAccessor, IProductRepository _productRepository, HttpContext context)
        {
            req.Headers.TryGetValue("ApiKey", out var ApiKey);

            if (ApiKey == "")
            {
                // Set CORS-related headers to block the request
                context.Response.Headers.Add("Access-Control-Allow-Origin", "null"); // You can customize the value
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, ApiKey"); // Add any custom headers needed

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Results.StatusCode(StatusCodes.Status403Forbidden);
            }

            var validate = _productRepository.Purchase(context, ApiKey);
            if (!validate)
            {
                // Set CORS-related headers to block the request
                context.Response.Headers.Add("Access-Control-Allow-Origin", "null"); // You can customize the value
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, ApiKey"); // Add any custom headers needed

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return null;
            }

            return Results.Ok(validate);
        }





        public async static Task<IResult> AddProduct(Product product, IProductRepository _productRepository,
     IAuthenticateRepository _service, HttpContext context, IHttpContextAccessor httpContextAccessor)
        {
            if (product == null) return Results.StatusCode(StatusCodes.Status404NotFound);
            var prduct = _productRepository.AddProduct(product);

            
            if (product == null) return Results.StatusCode(StatusCodes.Status404NotFound);
            return Results.Ok(product);
        }


    }


}
