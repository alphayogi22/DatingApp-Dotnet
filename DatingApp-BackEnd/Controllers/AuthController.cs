
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp_BackEnd.Data;
using DatingApp_BackEnd.Dto;
using DatingApp_BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp_BackEnd.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

                private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config )
        { 
             _config= config;
            _repo = repo;
        }

       [HttpPost("Register")]

       public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
       {
           userForRegister.username = userForRegister.username.ToLower();

           if (await _repo.UserExists(userForRegister.username))
               return BadRequest("Username  is taken");

            var userToCreate = new User
            {
                Username = userForRegister.username
            };

            var userCreated = _repo.Register(userToCreate, userForRegister.password);

            return StatusCode(201);
           
       }

       [HttpPost("login")]
       public async Task<IActionResult> login (UserForLoginDto userForLogin)
       {
           var loggedInUser = await _repo.login(userForLogin.Username.ToLower(), userForLogin.Password);


           if(loggedInUser == null)
              return Unauthorized();

           var claim = new[]
           { 
               new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
               new Claim(ClaimTypes.Name, loggedInUser.Username)         
           };

           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

           var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject = new ClaimsIdentity(claim),
               Expires = DateTime.Now.AddDays(1),
               SigningCredentials = cred
           };

           var tokenHandler = new JwtSecurityTokenHandler();

           var token = tokenHandler.CreateToken(tokenDescriptor);

           return Ok( new {
               token = tokenHandler.WriteToken(token)
           });
       }
    }
}