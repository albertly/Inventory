using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IAuthService
    {
        string GenerateToken(IAuthContainerModel model);
        bool IsTokenValid(string token);

        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
