using Amazon.Runtime.Internal;
using AuthenticationAPI.DTOS;
using AuthenticationAPI.Interfaces;
using AuthenticationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        //private readonly IMongoCollection<ApplicationUser> _users;


        public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
           // var database = mongoClient.GetDatabase(config.DatabaseName);
           // _users = database.GetCollection<ApplicationUser>(config.UserCollectionName);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            if(await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return BadRequest("Email is already registered");
            }
            if(await _userManager.FindByNameAsync(request.LoginId) != null)
            {
                return BadRequest("LoginId already exists");
            }
            ApplicationUser appUser = new ApplicationUser
            {

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.LoginId,
                PhoneNumber = request.ContactNumber,

            };

            appUser.SecurityStamp = Guid.NewGuid().ToString();

            //await _users.InsertOneAsync(appUser);

            var result = await _userManager.CreateAsync(appUser, request.Password);

            
           
            await _userManager.AddToRoleAsync(appUser, "Member");
            

            
           
            
            return Ok("Registration Successfull");          
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            bool check=false;
            var user = await _userManager.FindByNameAsync(request.LoginId);
            if(user != null)
            {
                check = await _userManager.CheckPasswordAsync(user, request.Password);
            }
            if (user == null || !check)
            {
                return BadRequest("Invalid LoginId/Password");
            }
            var token = await _tokenService.CreateToken(user);
            return Ok(new { loginId = user.UserName, jwtToken = token });
            
        }

        [HttpPut]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.LoginId);
            if (user == null) 
            {
                return BadRequest("User doesn't exists");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            return Ok("Password updated successfully");
        }

        
    }
}
