using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO.Entities;
using TODO.Security;
using TODO.TODO.DTOs;

namespace TODO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly TODOContext _context;
        private readonly JwtSettings _jwtSettings;
        public UserDetailsController(TODOContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }


        // GET: api/UserDetails
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetUserDetails()
        {
            return await _context.UserDetails.Include(u=> u.UserClaims).ToListAsync();
        }

        [Route("UserEmails")]
        [HttpGet]
        public async Task<ActionResult<string[]>> GetUserEmails()
        {
            return await _context.UserDetails.Select(u=> u.Email).ToArrayAsync();
        }

        // GET: api/UserDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetails>> GetUserDetails(int id)
        {
            var userDetails = await _context.UserDetails.Include(u=> u.UserClaims).Where(u=> u.Id==id).FirstOrDefaultAsync();

            if (userDetails == null)
            {
                return NotFound();
            }

            return userDetails;
        }

        // PUT: api/UserDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetails(int id, UserDetails userDetails)
        {
            if (id != userDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserDetails>> PostUserDetails(UserDetails userDetails)
        {
            try
            {
                userDetails.UserClaims = new List<UserClaims>() { new UserClaims() { ClaimType="canAccessTODO",ClaimValue=true},
                    new UserClaims() { ClaimType = "canAccessDashboard", ClaimValue = true },new UserClaims() { ClaimType="canAccessAdmin",ClaimValue=false}  };
               
                _context.UserDetails.Add(userDetails);
                await _context.SaveChangesAsync();

                return Ok(System.Net.HttpStatusCode.Created);
            }
            catch(Exception ex)
            {
                var a = ex;
                throw new Exception(ex.Message);
            }
        }

        // DELETE: api/UserDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDetails>> DeleteUserDetails(int id)
        {
            var userDetails = await _context.UserDetails.FindAsync(id);
            if (userDetails == null)
            {
                return NotFound();
            }

            _context.UserDetails.Remove(userDetails);
            await _context.SaveChangesAsync();

            return userDetails;
        }
        [Route("Login")]
        [HttpPost]
        public  IActionResult Login(UserDetails userDetails)
        {
           IActionResult ret;
           UserAuthenticationObject obj = new UserAuthenticationObject();
            SecurityManager security = new SecurityManager(_context,_jwtSettings);
            obj= security.ValidateUser(userDetails.Email, userDetails.Password);
            if(obj.IsAuthenticated)
            {
                ret = StatusCode((int)HttpStatusCode.OK,obj);
            }
            else
            {
                ret = StatusCode((int)HttpStatusCode.NotFound, "user not found");
            }
            return ret;
        }
        private bool UserDetailsExists(int id)
        {
            return _context.UserDetails.Any(e => e.Id == id);
        }
    }
}
