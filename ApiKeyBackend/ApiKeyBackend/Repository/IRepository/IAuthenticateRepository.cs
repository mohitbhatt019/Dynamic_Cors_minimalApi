using ApiKeyBackend.Models;

namespace ApiKeyBackend.Repository.IRepository
{
    public interface IAuthenticateRepository
    {
         Task<bool> IsUniqueUser(string username);
         Task<ApplicationUser> AuthenticateUser(LoginModel loginModel);
        Task<bool> RegisterUser(ApplicationUser registerUser);
        public Product GetProductByApiKey(string key);
        dynamic GetUsers(string id);
        public dynamic GetCors();
    }
}
