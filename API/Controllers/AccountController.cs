using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        //Adding the constructor so that we can inject our data context into this class.
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        //We can also define Register([FromBody]string userName, string password) or 
        //Register([FromQuery]string userName, string password) to get the params from 
        //either request body or query string.
        //Section4: Replacing the params (string userName, string password) with DTO
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //We need to use await when we are calling any async method
            if (await UserExists(registerDto.Username))
            {
                //we are able to return this BadRequestObjectResult with 400 error code 
                //as this Register method is expecting any ActionResult can be returned.
                return BadRequest("Username is taken");
            }
            //HMACSHA512()-Initializes a new instance of the HMACSHA512 class with a randomly generated key.
            //HMACSHA512 provides us with a hasing algoritham to create a password hash.
            //Here 'using' class is to dispose the resource, once finished using the class HMACSHA512.
            //Since this class HMACSHA512 inherits the IDisposable interface and having the Dispose() method defined in this class.
            //so, calling the dispose method with the reference of 'using'.
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            //Here just tracking the user entity now in Entity Framework.
            _context.Users.Add(user);
            //This task will call the database and saves all changes made in this context (user enitity) to the user table. 
            await _context.SaveChangesAsync();
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };        
            }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            if (user == null)
            {
                //Unauthorized method produces the response with 401 status code
                return Unauthorized("Invalid username");
            }
            //We are passing the same user's password salt as key to compute the hash of the user provided password.
            //So that we can compare and identify whether the user entered the correct password or not.
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            //comparing the user provided password's hashed data with the hashed password stored in DB.
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDto{
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };  
        }

        private async Task<bool> UserExists(string userName)
        {
            //We need to use await when we are calling any async method.
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}