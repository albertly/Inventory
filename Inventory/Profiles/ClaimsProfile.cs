using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Profiles
{
    public class ClaimsProfile : Profile
    {
   
            public ClaimsProfile()
            {
                CreateMap<Entities.Claim, Models.ClaimDto>();
                CreateMap<Models.ClaimForCreationDto, Entities.Claim>();
            }
        }
  
}
