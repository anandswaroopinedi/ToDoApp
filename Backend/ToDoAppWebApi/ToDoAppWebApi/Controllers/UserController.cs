using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DtoModels;
using Models.InputModels;
using messages = Common.Enums.Messages;

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
        [HttpPost("add")]
        public async Task<ResponseDto> AddUser(User user)
        {
            Boolean result = await _userManager.AddUser(user);
            if (result)
            {
                return new ResponseDto
                {
                    Status = (int)messages.Success,
                    Message = "Successfully registered",
                };
            }
            return new ResponseDto
            {
                Status = (int)messages.Failure,
                Message = "UserName Already Exists so choose other",
            };

        }
    }
}
