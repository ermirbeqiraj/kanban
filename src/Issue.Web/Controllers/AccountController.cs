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
    //TODO: AUTH
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;


        public AccountController(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        // list
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var dbItems = await _userManager.Users.Select(x => new UserForListModel
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName
            }).ToListAsync();
            return Ok(dbItems);
        }


        // create
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var aspNetUser = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(aspNetUser, model.Password);
                if (!result.Succeeded)
                {
                    // badreq
                    var err = result.Errors.Select(x => x.Description).FirstOrDefault();
                    return BadRequest(err);
                }
                else
                {
                    return Ok();
                }
            }
            else
            {
                // bad req
                return BadRequest("Model state is not valid");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int? id)
        {
            if(!id.HasValue)
            {
                return BadRequest("Model is not valid");
            }
            var user = await _userManager.Users.Where(x => x.Id == id).Select(a=> new UserUpdateModel
            {
                Id = a.Id,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                UserName = a.UserName
            }).FirstOrDefaultAsync();
           
            return Ok(user);
        }

        // update
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UserUpdateModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid");

            }
            var user = await _userManager.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();


            //model.Password = user.PasswordHash;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(x => x.Description).FirstOrDefault());
            else
                return Ok();
        }

        // delete
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();

            var result  = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(x => x.Description).FirstOrDefault());
            else
                return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword([FromBody]UpdateUserPassword model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid");

            var user = await _userManager.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();

            var removePwd = await _userManager.RemovePasswordAsync(user);
            if (!removePwd.Succeeded)
                return BadRequest("Failed to remove old user password");

            var addPws = await _userManager.AddPasswordAsync(user, model.Password);

            if (!addPws.Succeeded)
                return BadRequest(addPws.Errors.Select(x => x.Description).FirstOrDefault());
            else
                return Ok();
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