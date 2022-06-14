using FreeCourse.IdentityServer.Dtos;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]

   
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

  
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signupDto)
        {
            var user = new ApplicationUser
            {

                UserName = signupDto.UserName,
                Email = signupDto.Email,
                City = signupDto.City
            };

            var result = await _userManager.CreateAsync(user, signupDto.Password);
            if(!result.Succeeded)
            {
                //Eğer işlem başarısız ise
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(t => t.Description).ToList(), 400));
            }

            //Create işleminde neden NoContent dönüldü, ilerleyen videolarda değinebilir dikkatli dinle..
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userClaims = User.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub);
            if (userClaims == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userClaims.Value);
            if (user is null) return BadRequest();

            return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email, City = user.City }); 
        }
    }
}
