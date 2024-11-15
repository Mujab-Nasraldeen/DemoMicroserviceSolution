using JWTAuthenticationManager;
using JWTAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;

        public AccountController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        [HttpPost]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest req) 
        {
            var authenticateResp = _jwtTokenHandler.GenerateJwtToken(req);
            if (authenticateResp == null) return Unauthorized();
            return authenticateResp;
        }

    }
}
