using Issue.Web.IdentityModels;
using Issue.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Issue.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;


        public AccountController(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tokenKey = _config["Token:Key"];
                var tokenIssuer = _config["Token:Issuer"];

                var dbUser = await _userManager.Users.Where(x => x.UserName == model.Username || x.Email == model.Username)
                                                .Include(c => c.Claims)
                                                .FirstOrDefaultAsync();

                var authResult = await _userManager.CheckPasswordAsync(dbUser, model.Password);

                if (authResult)
                {
                    var roles = await _userManager.GetRolesAsync(dbUser);

                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.NameId, model.Username),
                    };

                    foreach (var item in roles)
                    {
                        claims.Add(new Claim("roles", item));
                        claims.Add(new Claim(item, item));
                    }

                    var otherClaims = dbUser.Claims.ToList();
                    foreach (var item in otherClaims)
                    {
                        claims.Add(new Claim(item.ClaimType, item.ClaimValue));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(tokenIssuer,
                        tokenIssuer,
                        claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: creds);

                    var response = new
                    {
                        access_token = new JwtSecurityTokenHandler().WriteToken(token),
                        expires_in = DateTime.Now.AddDays(7),
                        refresh_token = "",
                        roles
                    };

                    return Ok(response);
                }
            }
            return BadRequest("Your credentials are not valid!");
        }
    }
}