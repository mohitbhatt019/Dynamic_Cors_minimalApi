using ApiKeyBackend.Models;
using ApiKeyBackend.Repository.IRepository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace ApiKeyBackend.Repository
{
    public class AuthenticateRepository : IAuthenticateRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public AuthenticateRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<ApplicationUser> AuthenticateUser(LoginModel loginModel)
        {
            // Find the user with the given username
            var userExist = await _userManager.FindByNameAsync(loginModel.UserName);

            // Verify the user's password
            var VERIFY = await _signInManager.CheckPasswordSignInAsync(userExist, loginModel.Password, false);

            // If the password is correct, return the user
            if (!VERIFY.Succeeded) return null;

            return userExist;

        }

        [DisableCors]
         public dynamic GetCors()
        {
            var cors = _context.Products.ToList();
            if (cors != null)
            {
                foreach (var item in cors)
                {
                    Product product = new Product();
                    product.CorsPolicy=item.CorsPolicy;
                };
                return cors; 
            }
            return null;
        }

        public Product GetProductByApiKey(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var product = _context.Products.FirstOrDefault(x => x.ApiKey == key);
            return product;
        }

        public dynamic GetUsers(string id)
        {
            if (id == null) return null;
           var  users = _context.Users.FirstOrDefault(idd=>idd.Id==id);
            if(users!=null)
            {
                    ApplicationUser applicationUser = new ApplicationUser()
                    {
                        UserName = users.UserName,
                        Email = users.Email,
                    };

                return users;
            }
            return null;
        }

        public async Task<bool> IsUniqueUser(string username)
        {
            // Find a user with the given username
            var duplicateUser = await _userManager.FindByNameAsync(username);

            // If a user with the same username is found, return false
            if (duplicateUser != null) { return false; }

            // If no user with the same username is found, return true
            else { return true; }
        }

        public async Task<bool> RegisterUser(ApplicationUser registerUser)
        {
            // Create a new user with the given registerModel and password
            var user = await _userManager.CreateAsync(registerUser, registerUser.PasswordHash);
            //await _userManager.AddToRoleAsync(registerModel, UserRoles.Role_Admin);


            // If user creation fails, return false
            if (!user.Succeeded) return false;
            return true;
        }
    }
}
