using System;
using System.Security.Cryptography;
using CryptoHelper;
using HiddenSound.API.Areas.Application.Models;
using HiddenSound.API.Areas.Application.Repositories;
using HiddenSound.API.Areas.Shared.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HiddenSound.API.Areas.Application.Controllers
{
    [Route("Application/[controller]")]
    public class AuthController : ApplicationController
    {
        public IUserRepository UserRepository { get; set; }

        [HttpPost]
        [Route("[action]")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            var user = UserRepository.GetUser(request.Email, request.Password);

            if (user == null || !user.IsVerified)
            {
                return Unauthorized();
            }

            return Ok();
        }


        [HttpPost]
        [Route("[action]")]
        public ActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ModelStateError();
            }

            var user = UserRepository.GetUser(request.Email);

            if (user != null)
            {
                return BadRequest(nameof(Shared.Models.User.Email), "Email is already in use.");
            }

            var hashedPassword = Crypto.HashPassword(request.Password);

            user = UserRepository.CreateUser(new User
            {
                Email = request.Email,
                Password = hashedPassword,
                IsDeveloper = Convert.ToBoolean(request.IsDeveloper),
                IsVerified = true
            });

            return Created($"/Application/User/{user.ID}", user);
        }
    }
}
