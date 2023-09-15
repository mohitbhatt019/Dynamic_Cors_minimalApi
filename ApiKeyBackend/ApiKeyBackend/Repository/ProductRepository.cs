using ApiKeyBackend.Models;
using ApiKeyBackend.Repository.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiKeyBackend.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public ProductRepository(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public bool AddProduct(Product product)
        {
            if (product == null) { return false; }
            _context.Products.Add(product);
            _context.SaveChanges();
            return true;
        }

        public dynamic GetInnerCode(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        public Product GetProductByApiKey(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var product = _context.Products.FirstOrDefault(x => x.ApiKey == key);
            return product;
        }

        public bool Purchase(HttpContext context, string key)
        {
            //// Get the user's API key from the request headers
            //if (!context.Request.Headers.TryGetValue("ApiKey", out var apiKey))
            //{
            //    return false;
            //}


            var product = GetProductByApiKey(key);

            if (product == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(product.CorsPolicy) &&
                !Array.Exists(product.CorsPolicy.Split(','), origin => origin == context.Request.Headers["Origin"]))
            {
                return false;
            }


            return true;
        }

        public async static Task<IResult> ValidateApi(IAuthenticateRepository _service, HttpRequest req,
IHttpContextAccessor httpContextAccessor, IProductRepository _productRepository, HttpContext context)
        {

            req.Headers.TryGetValue("ApiKey", out var ApiKey);

            if (ApiKey == "")
            {
                // Set CORS-related headers to block the request
                context.Response.Headers.Add("Access-Control-Allow-Origin", ""); // Leave it empty
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, ApiKey"); // Add any
                                                                                                      //custom headers needed

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Results.StatusCode(StatusCodes.Status403Forbidden);
            }

            var validate = _productRepository.Purchase(context, ApiKey);
            if (!validate)
            {
                // Set CORS-related headers to block the request
                context.Response.Headers.Add("Access-Control-Allow-Origin", ""); // Leave it empty
                context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, ApiKey"); // Add any
                                                                                                      //custom headers needed

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Results.StatusCode(StatusCodes.Status403Forbidden);
            }

            return Results.Ok(validate);
        }

        
    }
}
