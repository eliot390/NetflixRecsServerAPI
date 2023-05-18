using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetflixModel;
using NetflixRecsServer.Dtos;
using System.IdentityModel.Tokens.Jwt;

namespace NetflixRecsServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase {
        private readonly UserManager<NetflixRecsUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public AccountController(UserManager<NetflixRecsUser> userManager, JwtHandler jwtHandler) {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest loginRequest) {
            NetflixRecsUser? user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password)) {
                return Unauthorized(new LoginResult {
                    Success = false,
                    Message = "Invalid Username or Password."
                });
            }

            JwtSecurityToken securityToken = await _jwtHandler.GetTokenAsync(user);
            string? jwt = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return Ok(new LoginResult {
                Success = true,
                Message = "Login Successful",
                Token = jwt
            });
        }
    }
}
