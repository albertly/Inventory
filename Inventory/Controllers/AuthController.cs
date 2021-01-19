using AutoMapper;
using Inventory.Models;
using Inventory.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService auth, IUserRepository userRepository)
        {
            _auth = auth;
            _userRepository = userRepository;
        }
        //comment
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost("Validate")]
        public bool Validate([FromBody] ValidateModel token)
        {
            return _auth.IsTokenValid(token.Token);
        }

        [HttpPost("Login")]
        public ActionResult Login([FromBody] AuthModel auth)
        {
            bool UserAuthenticate = false;
            if (auth == null)
            {
                return NotFound();
            }

            var email = auth.eMail;
            //var password = auth.Password;

            if (auth.Password.Length == 0)
            {
                return NotFound();
            }

            //Varify user password if DB request error then return Status500
            try
            {
                UserAuthenticate = _userRepository.varifuUserPassword(auth);
                if (UserAuthenticate == false)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            //----------------------------------------------------------------------
            string token = _auth.GenerateToken(new JWTContainerModel
            {
                ExpireMinutes = 3600,
                Claims = new Claim[]
                {
                   // new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email),
                    new Claim("Job", "CTO")
                }

            });

            return Ok(token);
        }


    }
}
