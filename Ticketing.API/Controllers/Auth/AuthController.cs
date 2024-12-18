using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ticketing.API.Model;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories.Auth;
using Ticketing.API.Repositories.Interfaces.Auth;
using Ticketing.API.Services;
using LoginRequest = Ticketing.API.Model.Dto.Requuest.LoginRequestDto;
using RegisterRequest = Ticketing.API.Model.Dto.Requuest.RegisterRequestDto;

namespace Ticketing.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IUserManagerRepository userManagerRepository;
        private readonly ITokenRepository tokenRepository;
        private readonly IMapper mapper;

        public AuthController(UserManager<User> userManager , 
            IUserManagerRepository userManagerRepository , 
            ITokenRepository tokenRepository ,
            IMapper mapper
        )
        {
            this.userManager = userManager;
            this.userManagerRepository = userManagerRepository;
            this.tokenRepository = tokenRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            var user = await userManagerRepository.RegisterUser(registerRequest);
            if (user == null) 
            {
                return BadRequest(new ApiResponse<string>()
                {
                    Success = false,
                    Message = "Registration Failed At The Moment",
                    Data = ""
                });
            }

            return Ok(new ApiResponse<User?>()
            {
                Success = true,
                Message = "User Registered Successfully",
                Data = user
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
        {
            var user = await userManagerRepository.GetUserByUserName(loginRequest.UserName);
            if (user == null)
            {
                return BadRequest(new ApiResponse<User?>()
                {
                    Success = false,
                    Message = "No user found with given email",
                    Data = user
                });
            }

            bool passwordMatch = await userManagerRepository.CheckPassword(user, loginRequest.Password);

            if(!passwordMatch)
            {
                return BadRequest(new ApiResponse<string>()
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
                return new ObjectResult(new ApiResponse<string>()
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

            var mappedUser = mapper.Map<UserResponseDto>(user);
            return Ok(new ApiResponse<object>()
            {
                Success = true,
                Message = "Login Sucessful",
                Data = new { Token =  jwtToken , Roles = roles , User = mappedUser }
            });

        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            
            return Ok(new ApiResponse<string>()
            {
                Success = true,
                Message = "Logout Sucessful",
                Data = ""
            });
        }
    }
}
