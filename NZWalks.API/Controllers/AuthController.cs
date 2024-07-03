using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository= tokenRepository;
        }

        //POST: /api/Auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if(identityResult.Succeeded)
            {
                //Add roles to this user
                if(registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if(identityResult.Succeeded)
                    {
                        return Ok("User was registered!. Please login");
                    }
                }

            }

            return BadRequest("Something went wrong");
        }

        //POST: /api/Auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user  = await userManager.FindByEmailAsync(loginRequestDto.UserName);
            if (user != null)
            {
                var passwordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (passwordResult)
                {
                    //get roles for this user
                    var roles = await userManager.GetRolesAsync(user);
                    if(roles.Count!=0)
                    {
                        //Create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }

                    return BadRequest("Username or password is incorrect");
                }
            }
            return BadRequest("Username or password is incorrect");
        }
    }
}
