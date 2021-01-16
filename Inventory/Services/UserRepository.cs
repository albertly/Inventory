using Inventory.DBContexts;
using Inventory.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLContext _context;

        public UserRepository(SQLContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public User GetUser(string userId)
        {
            //return _context.Users.Where<User>(u => u.Id == userId);
            return _context.Users.FirstOrDefault<User>(u => u.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
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
