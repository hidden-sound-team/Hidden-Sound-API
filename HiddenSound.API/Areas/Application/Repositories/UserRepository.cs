using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoHelper;
using HiddenSound.API.Areas.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HiddenSound.API.Areas.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        public HiddenSoundDbContext DbContext { get; set; }

        public User GetUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var users = DbContext.Users.AsNoTracking()
                .Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                            && Crypto.VerifyHashedPassword(u.Password, password))
                .ToList();

            return RemoveSensitiveData(users.FirstOrDefault());
        }

        public User GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var user = DbContext.Users.AsNoTracking()
                .Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return RemoveSensitiveData(user.FirstOrDefault());
        }

        public User CreateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentException();
            }

            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            return RemoveSensitiveData(user);
        }

        public User GetUser(int id)
        {
            var user = DbContext.Users
                .Where(u => u.ID == id)
                .ToList();

            return RemoveSensitiveData(user.FirstOrDefault());
        }

        private User RemoveSensitiveData(User user)
        {
            if (user != null)
            {
                user.Password = null;
            }
            
            return user;
        }
    }
}
