using Inventory.DBContexts;
using Inventory.Models;
using Inventory.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLContext _context;

        public UserRepository(SQLContext context)
        {            
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddClaim(string userId, Claim claim)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            
            claim.UserId = userId;
            _context.Claims.Add(claim);
        }

        public void AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }


            OptionsWrapper<PasswordHasherOptions> options = new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,                
            });

            PasswordHasher<User> hasher = new PasswordHasher<User>(options);

            user.Password = hasher.HashPassword(user, user.Password);

            user.IdGuid = Guid.NewGuid();

            _context.Users.Add(user);


        }

        public void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Claim> GetClaim(string userId, Guid claimId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (claimId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(claimId));
            }

                        
            return await _context.Claims
              .Where(c => c.UserId == userId && c.Id == claimId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync (IEnumerable<Guid> claimsIds)
        {
            return await _context.Claims.Where(c => claimsIds.Contains(c.Id)).ToListAsync();
        }

        public User GetUser(string userId)
        {
           // var f = _context.Users.Where<User>(u => u.Id == userId).ToListAsync<User>();
            
            return _context.Users.FirstOrDefault<User>(u => u.Id == userId);
        }

        public  async Task<IEnumerable<User>> GetUsers(string search)
        {
            //byte[] mybyte = { 12, 12, 45 };
            //List<byte> mylbyte = new List<byte> { 12, 23 };
            //mylbyte.Add(34);

            if (String.IsNullOrWhiteSpace(search))
            {
                return await _context.Users.Select(x => x).ToListAsync();
            }
            else
            {
                return await _context.Users.Where(x => x.FirstName.Contains(search)).ToListAsync();
            }

        }

        public async Task<IEnumerable<User>> GetUsersWithClaims()
        {

                return await _context.Users.Include(u => u.Claims).ToListAsync();

        }

        public bool Save()
        {            
            return (_context.SaveChanges() >= 0);
        }

        public async Task<bool> SaveAsync()
        {
            return ( await _context.SaveChangesAsync() >= 0);
        }
        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        public bool UserExists(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _context.Users.Any(u => u.Id == userId);
        }
        public bool varifuUserPassword(AuthModel authModel)
        {

            if (authModel == null)
            {
                throw new ArgumentNullException(nameof(authModel));
            }
            PasswordVerificationResult retVar;
            AuthModel userInfo;

            //user = _context.Users.FirstOrDefault<User>(u => u.Email == authModel.eMail);
            //Get login user password from DB
            userInfo= _context.Users.Where(u => u.Email == authModel.eMail).Select(u => new AuthModel
            {
                eMail = u.Email,
                Password = u.Password
                
            }).SingleOrDefault();
            if (userInfo == null) return false;

            OptionsWrapper<PasswordHasherOptions> options = new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
            });

            PasswordHasher<AuthModel> hasher = new PasswordHasher<AuthModel>(options);
            //compare 2 password
            retVar = hasher.VerifyHashedPassword(authModel, userInfo.Password, authModel.Password);

            if (retVar == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
