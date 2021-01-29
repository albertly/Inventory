using AutoMapper;
using Inventory.Models;
using Inventory.Entities;
using Inventory.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UserController( IUserRepository userRepository, IMapper mapper)
        {
           
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("User")]
        public ActionResult<Models.UserDto> AddUser([FromBody] UserForCreationDto userCDto)
        {
            var user = _mapper.Map<User>(userCDto);

            _userRepository.AddUser(user);
            _userRepository.Save();

            var userDto = _mapper.Map<UserDto>(user);

            return CreatedAtRoute("GetUser", new { Id = user.Id }, userDto);
        }


        [HttpGet("Users", Name = "GetUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string search)
        {

            var users = await _userRepository.GetUsers(search);

            if (users == null)
            {
                return NotFound();
            }

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);


            return Ok(usersDto);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Policy = "Manager")]
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
