using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using School_Administration.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Linq;
using School_Administration.Dtos;
using System.Threading.Tasks;
using School_Administration.Models;
using School_Administration.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace School_Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly IRepositoryWrapper _repository;

        public LoginController(IConfiguration config, IRepositoryWrapper repository)
        {
            _config = config;
            _repository = repository;
        }

        [HttpPut("RegisterUser")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> Register(UserDto dto)
        {

            var checkUserExist = (await _repository.UserRepository.GetAllEntity(w =>
            w.UserName == dto.UserName)).SingleOrDefault();

            if (checkUserExist != null)
                return Ok("User Already exist");

            var role = (await _repository.RoleRepository.GetAllEntity(w =>
            w.RoleName == dto.RoleName)).SingleOrDefault();

            if (role == null)
                return Ok("Role doesn't exist");

            var user = new User()
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                Password = dto.Password
            };

            user.RoleId = role.RoleId;

            _repository.UserRepository.Add(user);

            await _repository.SaveAsync();

            return Ok("User Added successfully");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            IActionResult response = Unauthorized();
            User user = await AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJWTToken(user);
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = user,
                });
            }
            return response;
        }
       async Task<User> AuthenticateUser(LoginUserDto loginCredentials)
        {
            var user = (await _repository.UserRepository.GetAllEntity(w =>
            w.UserName == loginCredentials.UserName && w.Password == loginCredentials.Password, w =>
            w.Include(u => u.Role))).SingleOrDefault();

            return user;
        }
        string GenerateJWTToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt")["SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                  new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                  new Claim("fullName", userInfo.FullName.ToString()),
                  new Claim("role",userInfo.Role.RoleName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };

            var token = new JwtSecurityToken(
            issuer: _config.GetSection("Jwt")["Issuer"],
            audience: _config.GetSection("Jwt")["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
