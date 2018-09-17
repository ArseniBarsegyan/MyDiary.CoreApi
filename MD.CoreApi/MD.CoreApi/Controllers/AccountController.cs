﻿//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using IdentityModel.Client;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using MD.CoreApi.Dto;
//using MD.Identity;

//namespace MD.CoreApi.Controllers
//{
//    [Route("[controller]/[action]")]
//    public class AccountController : Controller
//    {
//        private readonly SignInManager<AppUser> _signInManager;
//        private readonly UserManager<AppUser> _userManager;
//        private readonly IConfiguration _configuration;

//        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
//            IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _configuration = configuration;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginDto model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var tokenClient = new TokenClient("http://localhost:52075/connect/token", "MDMVC", "MDSecretKey");
//            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("MD.CoreApi");

//            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

//            if (result.Succeeded)
//            {
//                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
//                return Ok(GenerateJwtToken(model.Email, appUser));
//            }
//            return NotFound();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register([FromBody] RegisterDto model)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var user = new AppUser
//            {
//                UserName = model.Email,
//                Email = model.Email
//            };
//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (result.Succeeded)
//            {
//                await _signInManager.SignInAsync(user, false);
//                return Ok(GenerateJwtToken(model.Email, user));
//            }
//            return BadRequest();
//        }

//        private object GenerateJwtToken(string email, AppUser user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, email),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(ClaimTypes.NameIdentifier, user.Id)
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

//            var token = new JwtSecurityToken(
//                _configuration["JwtIssuer"],
//                _configuration["JwtIssuer"],
//                claims,
//                expires: expires,
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}