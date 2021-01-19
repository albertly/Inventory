﻿using Inventory.Models;
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
        Task<bool> SaveAsync();

        bool varifuUserPassword(AuthModel authModel);

        void AddClaim(string userId, Claim claim);

        Task<bool> UserExistsAsync(string userId);
        bool UserExists(string userId);

        Task<Claim> GetClaim(string userId, Guid claimId);
    }
}
