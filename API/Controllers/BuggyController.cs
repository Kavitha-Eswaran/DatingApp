using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            //We are not having any user with id as -1. so the thing variable will be null.
            //When we try to convert null to ToString(), this will throw the exception.
            //Handling exception here using try, catch block is old fashion. so we can use our own exception handling middleware. i.e Globally we can use to handle our exceptions and whatever exception we get, we're going to return it in a standard way. What we're in development is going to be kind of similar to that developer exception page (commented out in startup.cs file). But we're also handle what we're going to send back for production as well.
            //So created a new folder 'Errors' & 'Middleware' with the required class files.
            var thingToReturn = thing.ToString();
            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}