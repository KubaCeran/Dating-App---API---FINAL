using Dating_App___API.Data;
using Dating_App___API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dating_App___API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
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
            else return Ok();
        }

        
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();
            return thingToReturn; //null refference exception
        }

        
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("Bad request mate");
        }
    }
}
