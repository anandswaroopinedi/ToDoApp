using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using ToDoAppWebApi.NewFolder;

namespace ToDoAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("authenticate")]
        public async Task<ApiResponse> AuthenticateUser(UserDto user)
        {
            int result = await _userManager.AuthenticateUser(user);
            if (result == (int)ResponseMessages.Messages.Success)
            {
                return new ApiResponse
                {
                    Status = (int)ResponseMessages.Messages.Success,
                    Message = "successfully logged in",
                    Result = await _userManager.GenerateToken(result)
                };
            }
            else if (result == 3)
            {
                return new ApiResponse
                {
                    Status = 3,
                    Message = "Password is Incorrect",
                };
            }
            else
            {
                return new ApiResponse
                {
                    Status = 4,
                    Message = "UserName incorrect check the username",
                };
            }
        }
        [HttpPost("add")]
        public async Task<ApiResponse> AddUser(UserDto user)
        {
            int result = await _userManager.AddUser(user);
            if (result == 1)
            {
                return new ApiResponse
                {
                    Status = (int)ResponseMessages.Messages.Success,
                    Message = "Successfully registered",
                };
            }
            return new ApiResponse
            {
                Status = 3,
                Message = "UserName Already Exists so choose other",
            };

        }
    }
}
