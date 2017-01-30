using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiddenSound.API.Areas.Shared.Models;
using Microsoft.CodeAnalysis.Semantics;

namespace HiddenSound.API.Areas.Application.Repositories
{
    public interface IUserRepository
    {
        User GetUser(int id);

        User GetUser(string email, string password);

        User GetUser(string email);

        User CreateUser(User user);
    }
}
