﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Auth;
using Ticketing.API.Repositories.Interfaces.Auth;
using Ticketing.API.Services;
using LoginRequest = Ticketing.API.Model.Dto.LoginRequest;
using RegisterRequest = Ticketing.API.Model.Dto.RegisterRequest;

namespace Ticketing.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserManagerRepository userManagerRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly SignInManager<IdentityUser> signInManager;

        public AuthController(UserManager<IdentityUser> userManager , 
            IUserManagerRepository userManagerRepository , 
            ITokenRepository tokenRepository ,
            SignInManager<IdentityUser> signInManager
        )
        {
            this.userManager = userManager;
            this.userManagerRepository = userManagerRepository;
            this.tokenRepository = tokenRepository;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            var identityUser = await userManagerRepository.RegisterUser(registerRequest);
            if (identityUser == null) 
            {
                return BadRequest(new ApiResponseService<string>()
                {
                    Success = false,
                    Message = "Registration Failed At The Moment",
                    Data = ""
                });
            }

            return Ok(new ApiResponseService<IdentityUser?>()
            {
                Success = true,
                Message = "User Registered Successfully",
                Data = identityUser
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        {
            var user = await userManagerRepository.GetUserByUserName(loginRequest.UserName);
            if (user == null)
            {
                return BadRequest(new ApiResponseService<string>()
                {
                    Success = false,
                    Message = "No user found with given email",
                    Data = ""
                });
            }

            bool passwordMatch = await userManagerRepository.CheckPassword(user, loginRequest.Password);

            if(!passwordMatch)
            {
                return BadRequest(new ApiResponseService<string>()
                {
                    Success = false,
                    Message = "Password Doesn't Match",
                    Data = ""
                });
            }

            //Generate Token
            var roles = await userManagerRepository.GetRoles(user);

            if (roles == null) 
            {
                return new ObjectResult(new ApiResponseService<string>()
                {
                    Success = false,
                    Message = "User doesn't have a defined role",
                    Data = ""
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

            var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

            return Ok(new ApiResponseService<object>()
            {
                Success = true,
                Message = "Login Sucessful",
                Data = new { Token =  jwtToken }
            });

        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return Ok(new ApiResponseService<string>()
            {
                Success = true,
                Message = "Logout Sucessful",
                Data = ""
            });
        }
    }
}