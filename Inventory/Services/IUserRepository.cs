using Inventory.Models;
using Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IUserRepository
    {

        User GetUser(string userId);

        Task<IEnumerable<User>> GetUsers(string search);

        Task<IEnumerable<User>> GetUsersWithClaims();

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(string userId);
        bool Save();
        Task<bool> SaveAsync();

        bool varifuUserPassword(AuthModel authModel);

        void AddClaim(string userId, Claim claim);

        Task<bool> UserExistsAsync(string userId);
        bool UserExists(string userId);

        Task<IEnumerable<Claim>> GetClaimsAsync(IEnumerable<Guid> claimsIds);

        Task<Claim> GetClaim(string userId, Guid claimId);
    }
}
