using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Entities;
using WebApplication1.Exceptions;
using WebApplication1.Models;
using WebApplication1.Models.Api;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        public static User user = new();
        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            return Ok(new ApiResponse<object>(201,new {user.Username},"User registered Successfully"));
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto request)
        {
            if (user.Username != request.Username)
            {
                throw new ApiException(404, "User not found");
            }

            var passwordValid =
                new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordValid == PasswordVerificationResult.Failed)
            {
                throw new ApiException(401, "Invalid credentials");
            }

            string token = CreateToken(user);

            return Ok(new ApiResponse<string>(
                200,
                token,
                "Login successful"
            ));
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Appsetting:Issuer"),
                audience: configuration.GetValue<string>("Appsetting:Audience"),
                claims:claims,
                expires:DateTime.UtcNow.AddDays(1),
                signingCredentials:creds

                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}

  
