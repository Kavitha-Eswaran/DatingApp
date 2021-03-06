using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    //Section2:
    //This is synchronous code
    //This attribute indicates that this class is of type APICotroller.
    //[ApiController]
    //Route indicates that how is the user going to get the api controller from the client
    //[Route("api/[controller]")]
   
    //The controller class should always inherit the ControllerBase
    
    //Section4: 
    // To avoid defining the above attributes in all controller files, created a common controller file.
    // Moved the above two attributes [ApiController], [Route] to BaseApiController file 
    // and inherting it from this BaseApiController controller instead of ControllerBase here. 
    public class UsersController : BaseApiController
    {
        //We need to get data from database. So for that, we need to use dependency injection.
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        //We just need to send a simple list of users to client. so we can prefer IEnumerable in this case.
        //List provides more features/methods to search/sort/manipulate lists.
        [HttpGet]
        // public ActionResult<IEnumerable<AppUser>> GetUsers()
        // {//This is synchronous method call and it is not a best practice to do. So create the async method for this.
        //     return _context.Users.ToList();
        // }
          public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
         {
             return await _context.Users.ToListAsync();
         }
        
        //Setting the below [Authorize] attribute to protect our GetUser endpoint.
        //[AllowAnonymous] attribute to allow all the users can access this endpoint.
        [Authorize]
        [HttpGet("{id}")]
        //public ActionResult<AppUser> GetUser(int id)
        //{//This is synchronous method call and it is not a best practice to do. So create the async method for this.
         //  return _context.Users.Find(id);
        //}
         public async Task<ActionResult<AppUser>> GetUser(int id)
        {
           return await _context.Users.FindAsync(id);
        }
    }
}