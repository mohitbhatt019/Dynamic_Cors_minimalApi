using ApiKeyBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiKeyBackend.Repository.IRepository
{
    public interface IProductRepository
    {
        public dynamic GetInnerCode(ApplicationUser applicationUser);
        public bool Purchase(HttpContext context, string key);
        public Product GetProductByApiKey(string key);

        public bool AddProduct(Product product);
    }
}
