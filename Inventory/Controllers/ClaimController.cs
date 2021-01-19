using AutoMapper;
using Inventory.Models;
using Inventory.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Controllers
{
    //http://localhost:5000/api/users/18/claim

    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ClaimController(IUserRepository userRepository, IMapper mapper)
        {
            
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ClaimDto>> CreateClaimForUser(string userId,ClaimForCreationDto claim)
        {
            if (!_userRepository.UserExists(userId))
            {
                return NotFound();
            }

            var claimEntity = _mapper.Map<Entities.Claim>(claim);
            _userRepository.AddClaim(userId, claimEntity);
            await _userRepository.SaveAsync();

            var claimToReturn = _mapper.Map<ClaimDto>(claimEntity);
            return CreatedAtRoute("GetClaimForUser",
                new { userId, claimId = claimToReturn.Id },
                claimToReturn);
        }

        //http://localhost:5000/api/users/18/claim/1823-32423432-432343
        [HttpGet("{claimId}", Name = "GetClaimForUser")]
        public async Task<ActionResult<ClaimDto>> GetClaimForUser(string userId, Guid claimId)
        {
            if (! await _userRepository.UserExistsAsync(userId))
            {
                return NotFound();
            }

            var claim = await _userRepository.GetClaim(userId, claimId);

            if (claim == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClaimDto>(claim));
        }
    }
}
