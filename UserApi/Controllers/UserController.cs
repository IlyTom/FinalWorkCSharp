using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.UserModels;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using UserApi.Abstraction;
using UserApi.RSAModel;
using UserApi.Utility;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserAccount _account;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, UserAccount account, IConfiguration configuration)
        {
            _userService = userService;
            _account = account;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([Description("Login user"), FromBody] LoginModel model)
        {
            if (!CheckEmail.Check(model.Email)) return BadRequest($"Email: {model.Email} - Invalid format");

            if (_account.GetToken == null) return BadRequest("You are already authorized");

            var response = _userService.Authentificate(model);

            if (!response.IsSuccess) return NotFound(response.Errors.FirstOrDefault()?.Message);

            _account.Login(response.Users[0]);
            _account.UpdateToken(GenerateToken(_account));

            return Ok(_account.GetToken());

        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("AddUser")]
        public ActionResult AddUser(RegistrationModel model)
        {
            if (!CheckEmail.Check(model.Email)) return BadRequest($"Email:'{model.Email}' - Invalid Format");
                
            if (!CheckPassword.Check(model.Password)) return BadRequest($"Password:'{model.Password}' - Invalid Format");

            var response = _userService.AddUser(model);

            if (!response.IsSuccess) return BadRequest(response.Errors.FirstOrDefault()?.Message);
               

            return Ok(response.UserId);
        }

        [AllowAnonymous]
        [HttpPost("AddAdmin")]
        public ActionResult AddAdmin(RegistrationModel model)
        {
            if (!CheckEmail.Check(model.Email)) return BadRequest($"Email:'{model.Email}' - Invalid Format");
                
            if (!CheckPassword.Check(model.Password)) return BadRequest($"Password:'{model.Password}' - Invalid Format"); 

            var response = _userService.AddAdmin(model);
            if (!response.IsSuccess) return BadRequest(response.Errors.FirstOrDefault()?.Message);
                

            return Ok(response.UserId);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("GetUsers")]
        public ActionResult GetUsers()
        {
            var response = _userService.GetUsers();
            if (!response.IsSuccess) return BadRequest(response.Errors.FirstOrDefault()?.Message);
                
            return Ok(response.Users);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost("GetOneUser")]
        public ActionResult GetUser(Guid? userId, string? email)
        {
            var response = _userService.GetUser(userId, email);
            
            if (!response.IsSuccess) return BadRequest(response.Errors.FirstOrDefault()?.Message);                

            return Ok(response.Users);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("DeleteUser")]
        public ActionResult DeleteUser(Guid? userId, string? email)
        {
            var response = _userService.DeleteUser(userId, email);
            
            if (!response.IsSuccess) return BadRequest(response.Errors.FirstOrDefault()?.Message);                

            return Ok();
        }

        [HttpPost("LogOut")]
        public ActionResult LogOut()
        {
            _account.Logout();
            return Ok();
        }

        private string GenerateToken(UserAccount model)
        {
            var key = new RsaSecurityKey(RSAService.GetPrivateKey());
            var credential = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.Role.ToString()),
                new Claim("UserId", model.Id.ToString())
            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credential);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
