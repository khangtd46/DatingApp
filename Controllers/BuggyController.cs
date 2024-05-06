using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    public class BuggyController : ApiController
    {   
        private readonly AppDBContext _dbContext;
        public BuggyController(AppDBContext context)
        {
            _dbContext = context;
        }


        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "This is my Secret Key";
        }
        [HttpGet("not-found")]
        public ActionResult<SystemUser> GetNotFound()
        {
            var thing = _dbContext.Users.Find(-1);
            if (thing != null) return thing;
            else
            {
                return NotFound();
            }
        }
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _dbContext.Users.Find(-1);
            var thingToString = thing.ToString();
            return thingToString;
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest("This was a bad request");
        }
    }
}
