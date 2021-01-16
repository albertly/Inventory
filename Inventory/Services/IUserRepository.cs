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
        IEnumerable<User> GetUsers();

        User GetUser(string userId);

        void AddUser(User user);

        void UpdateUser(User user);

        void DeleteUser(string userId);
        bool Save();

        bool varifuUserPassword(AuthModel authModel);

        void AddClaim(string userId, Claim claim);

        bool UserExists(string userId);

        Claim GetClaim(string userId, Guid claimId);
    }
}
