using AuthWebApi.Models;
using AuthWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase

    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
       
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if(!result.IsAuthenticated) return BadRequest(result.Message);

           
            return Ok(result);
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(model);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);



        }


    }
}
