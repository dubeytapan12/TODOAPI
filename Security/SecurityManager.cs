using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TODO.Entities;
using TODO.TODO.DTOs;

namespace TODO.Security
{
    public class SecurityManager
    {
        private readonly TODOContext _context;
        private readonly JwtSettings _jwtSettings;
        public SecurityManager(TODOContext context, JwtSettings jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public UserAuthenticationObject ValidateUser(string userName, string password)
        {
            UserAuthenticationObject obj = new UserAuthenticationObject();       
                var userDetail = _context.UserDetails.Include(u=>u.UserClaims).Where(u => u.Email == userName && u.Password == password).FirstOrDefault();
                obj = BuildUserAuthObject(userDetail);
           return obj;
        }

        private UserAuthenticationObject BuildUserAuthObject(UserDetails userDetails)
        {
            UserAuthenticationObject obj = new UserAuthenticationObject();
            if (userDetails != null)
            {
                obj.IsAuthenticated = true;
                obj.UserName = userDetails.Email;
                
                foreach (UserClaims claim in userDetails.UserClaims)
                {
                    typeof(UserAuthenticationObject).GetProperty(claim.ClaimType).SetValue(obj, Convert.ToBoolean(claim.ClaimValue));
                }
                obj.BearerToken = BuildJwtToken(obj);
            }

            
            return obj;
        }

        private string BuildJwtToken(UserAuthenticationObject authObj)
        {
            List<Claim> jwtClaim = new List<Claim>();

            jwtClaim.Add(new Claim(JwtRegisteredClaimNames.Sub, authObj.UserName));
            jwtClaim.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));


            jwtClaim.Add(new Claim("isAuthenticated", authObj.IsAuthenticated.ToString().ToLower()));
            jwtClaim.Add(new Claim("canAccessAdmin", authObj.canAccessAdmin.ToString().ToLower()));
            jwtClaim.Add(new Claim("canAccessDashboard", authObj.canAccessDashboard.ToString().ToLower()));
            jwtClaim.Add(new Claim("canAccessTODO", authObj.canAccessTODO.ToString().ToLower()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: jwtClaim, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddMinutes(_jwtSettings.MinToExpiration), signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
         return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
