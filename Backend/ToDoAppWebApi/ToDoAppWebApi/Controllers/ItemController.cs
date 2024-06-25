using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using ToDoAppWebApi.NewFolder;

namespace ToDoAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemManager _itemManager;
        public ItemController(IItemManager itemManager)
        {
            _itemManager = itemManager;
        }
        [HttpPost("create")]
        public async Task<ApiResponse> AddItem(ItemDto item)
        {
            item.Userid = ClaimsIdentifier.getIdFromToken(HttpContext);
            int result = await _itemManager.AddItem(item);
            if (result == (int)ResponseMessages.Messages.Success)
            {
                return new ApiResponse
                {
                    Status = (int)ResponseMessages.Messages.Success,
                    Message = "Task Added Successfully"
                };

            }
            else
            {
                return new ApiResponse
                {
                    Status = 3,
                    Message = "Task already exists"
                };
            }
        }
        [HttpGet("all")]
        public async Task<ApiResponse> GetAllItems()
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            return new ApiResponse
            {
                Status = (int)ResponseMessages.Messages.Success,
                Message = ResponseMessages.Messages.Success.GetEnumDescription(),
                Result = await _itemManager.GetAll(userId)
            };
        }
        [HttpDelete("delete")]
        public async Task<ApiResponse> DeleteItem(int id)
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            int result = await _itemManager.DeleteItem(id, userId);
            if (result == (int)ResponseMessages.Messages.Success)
            {
                return new ApiResponse
                {
                    Status = (int)ResponseMessages.Messages.Success,
                    Message = "Task Deleted from the list successfully"
                };
            }
            else
            {
                return new ApiResponse
                {
                    Status = 3,
                    Message = "Task Id doesn't exist"
                };
            }
        }
        [HttpPut("update")]
        public async Task<ApiResponse> UpdateItem(ItemDto item)
        {
            item.Userid = ClaimsIdentifier.getIdFromToken(HttpContext);
            await _itemManager.UpdateItem(item);
            return new ApiResponse
            {
                Status = (int)ResponseMessages.Messages.Success,
                Message = "Updated successfully",
            };
        }
        [HttpGet("active-items")]
        public async Task<ApiResponse> GetActiveItems()
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            return new ApiResponse { Status = (int)ResponseMessages.Messages.Success, Message = ResponseMessages.Messages.Success.GetEnumDescription(), Result = await _itemManager.GetActiveItems(userId) };
        }
        [HttpGet("completed-items")]
        public async Task<ApiResponse> GetCompletedItems()
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            return new ApiResponse { Status = (int)ResponseMessages.Messages.Success, Message = ResponseMessages.Messages.Success.GetEnumDescription(), Result = await _itemManager.GetCompletedItems(userId) };
        }
        [HttpDelete("delete-all")]
        public async Task<ApiResponse> DeleteAllItems()
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            _itemManager.DeleteItems(userId);
            return new ApiResponse
            {
                Status = (int)ResponseMessages.Messages.Success,
                Message = "All Tasks Deleted from the your list successfully"
            };
        }
        [HttpGet("completion-percentage")]
        public async Task<ApiResponse> CompletionPercentage()
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            return new ApiResponse
            {
                Status = (int)ResponseMessages.Messages.Success,
                Message = ResponseMessages.Messages.Success.GetEnumDescription(),
                Result = await _itemManager.CompletionPercentage(userId)
            };
        }
        [HttpPost("completed")]
        public async Task<ApiResponse> makeItemCompleted([FromBody] int id)
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            int result = await _itemManager.makeItemCompleted(id, userId);
            if (result == (int)ResponseMessages.Messages.Success)
            {
                return new ApiResponse
                {
                    Status = (int)ResponseMessages.Messages.Success,
                    Message = "Task Updated as completed"
                };
            }
            else
            {
                return new ApiResponse
                {
                    Status = 3,
                    Message = "Task not found"
                };
            }
        }
        [HttpPost("active")]
        public async Task<ApiResponse> makeItemActive([FromBody] int id)
        {
            int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            int result = await _itemManager.makeItemActive(id, userId);
            if (result == (int)ResponseMessages.Messages.Success)
            {
                return new ApiResponse
                {
                    Status = (int)ResponseMessages.Messages.Success,
                    Message = "Task Updated as active"
                };
            }
            else
            {
                return new ApiResponse
                {
                    Status = 3,
                    Message = "Task not found"
                };
            }
        }
    }
}
