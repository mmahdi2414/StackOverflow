using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MTProject.Models
{
    public class JWT
    {
        public IConfiguration Configuration { get; }
        public JWT(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string GenerateJwtToken(User user, string[] roleClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (string role in roleClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string s = Configuration["Configuration:JwtKey"];
            byte[] Bytekey = Convert.FromBase64String(Configuration["Configuration:JwtKey"]);
            var key = new SymmetricSecurityKey(Bytekey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration["Configuration:JwtExpireDays"]));

            var token = new JwtSecurityToken(
                null,
                null,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            //return token;
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
