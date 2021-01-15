using AutoMapper;
using Inventory.Models;
using Inventory.Services;
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
        private readonly IMapper _mapper;
        public AuthController(IAuthService auth, IUserRepository userRepository, IMapper mapper)
        {
            _auth = auth;
            _userRepository = userRepository;
            _mapper = mapper;
        }

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

        [HttpPost("JWT")]
        public string JWT([FromBody] AuthModel auth)
        {
            var name = auth.UserName;
            var email = "albert@yande.ru";

            string token = _auth.GenerateToken(new JWTContainerModel
            {
                ExpireMinutes = 3600,
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email)
                }

            });

            return token;
        }

        [HttpPost("User")]
        public ActionResult<UserDto> AddUser([FromBody] UserForCreationDto userCDto)
        {
            var user = _mapper.Map<User>(userCDto);

            _userRepository.AddUser(user);
            _userRepository.Save();

            var userDto = _mapper.Map<UserDto>(user);

            return CreatedAtRoute("GetUser", new { Id = user.Id }, userDto);
        }

        [HttpGet("User", Name = "GetUser")]
        public ActionResult<UserDto> GetUser([FromQuery] string Id)
        {
            var user = _userRepository.GetUser(Id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);

            
            return Ok(userDto);
        }
    }
}
