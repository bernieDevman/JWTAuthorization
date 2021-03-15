using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("Granny", "Cookie")
            };

            var secreteBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secreteBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;


            var signingCredentials = new SigningCredentials(key, algorithm );

            var token = new JwtSecurityToken(
                    Constants.Issuer,
                    Constants.Audience,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {accessToken = tokenJson});
        }
    }
}
