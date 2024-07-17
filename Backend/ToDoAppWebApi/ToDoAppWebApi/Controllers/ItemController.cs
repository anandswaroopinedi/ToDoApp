using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common;
using messages = Common.Enums.Messages;
using Models.InputModels;
using Models.DtoModels;

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
        [HttpPost]
        public async Task<ResponseDto> AddItem(Item addItemRequest)
        {
            addItemRequest.Userid = HttpContext.GetIdFromToken();
            int result = await _itemManager.AddItem(addItemRequest);
            if (result>0)
            {
                return new ResponseDto
                {
                    Status = (int)messages.Success,
                    Message = "Task Added Successfully",
                    Result= result
                };

            }
            else
            {
                return new ResponseDto
                {
                    Status = (int)messages.Failure,
                    Message = "Task already exists"
                };
            }
        }
        [HttpGet]
        public async Task<ResponseDto> GetAllItems()
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto
            {
                Status = (int)messages.Success,
                Message = messages.Success.GetEnumDescription(),
                Result = await _itemManager.GetAll(userId)
            };
        }
        [HttpDelete]
        public async Task<ResponseDto> DeleteItem([FromQuery]int idRequest)
        {
            int userId = HttpContext.GetIdFromToken();
            bool result = await _itemManager.DeleteItem(idRequest, userId);
            if (result)
            {
                return new ResponseDto
                {
                    Status = (int)messages.Success,
                    Message = "Task Deleted from the list successfully"
                };
            }
            else
            {
                return new ResponseDto
                {
                    Status = (int)messages.Failure,
                    Message = "Task Id doesn't exist"
                };
            }
        }
        [HttpPut]
        public async Task<ResponseDto> UpdateItem(Item updateItemRequest)
        {
            updateItemRequest.Userid = HttpContext.GetIdFromToken();
            await _itemManager.UpdateItem(updateItemRequest);
            return new ResponseDto
            {
                Status = (int)messages.Success,
                Message = "Updated successfully",
            };
        }
        [HttpGet("active-items")]
        public async Task<ResponseDto> GetActiveItems()
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto { Status = (int)messages.Success, Message =messages.Success.GetEnumDescription(), Result = await _itemManager.GetActiveItems(userId) };
        }
        [HttpGet("completed-items")]
        public async Task<ResponseDto> GetCompletedItems()
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto { Status = (int)messages.Success, Message =messages.Success.GetEnumDescription(), Result = await _itemManager.GetCompletedItems(userId) };
        }
        [HttpDelete("all")]
        public async Task<ResponseDto> DeleteAllItems()
        {
            int userId = HttpContext.GetIdFromToken();
            _itemManager.DeleteItems(userId);
            return new ResponseDto
            {
                Status = (int)messages.Success,
                Message = "All Tasks Deleted Successfully"
            };
        }
        [HttpGet("completion-percentage")]
        public async Task<ResponseDto> CompletionPercentage()
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto
            {
                Status = (int)messages.Success,
                Message = messages.Success.GetEnumDescription(),
                Result = await _itemManager.CompletionPercentage(userId)
            };
        }
        [HttpPost("completed")]
        public async Task<ResponseDto> makeItemCompleted([FromBody] int idRequest)
        {
            int userId = HttpContext.GetIdFromToken();
            bool result = await _itemManager.makeItemCompleted(idRequest, userId);
            if (result)
            {
                return new ResponseDto
                {
                    Status = (int)messages.Success,
                    Message = "Task Updated as completed"
                };
            }
            else
            {
                return new ResponseDto
                {
                    Status = (int)messages.Failure,
                    Message = "Task not found"
                };
            }
        }
        [HttpPost("active")]
        public async Task<ResponseDto> makeItemActive([FromBody] int id)
        {
            int userId = HttpContext.GetIdFromToken();
            bool result = await _itemManager.makeItemActive(id, userId);
            if (result)
            {
                return new ResponseDto
                {
                    Status = (int)messages.Success,
                    Message = "Task Updated as active"
                };
            }
            else
            {
                return new ResponseDto
                {
                    Status = (int)messages.Failure,
                    Message = "Task not found"
                };
            }
        }
        [HttpGet("pending-items")]
        public async Task<ResponseDto> GetPendingTasks(string property, string order)
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto { Status = (int)messages.Success, Message = messages.Success.GetEnumDescription(), Result = await _itemManager.GetPendingTasks(userId, property, order) };
        }
        [HttpGet("notify-items")]
        public async Task<ResponseDto> GetNotifyTasks()
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto { Status = (int)messages.Success, Message = messages.Success.GetEnumDescription(), Result = await _itemManager.GetNotifyTasks(userId) };
        }
        [HttpGet("notify-further-items")]
        public async Task<ResponseDto> GetFurtherNotifyTasks()
        {
            int userId = HttpContext.GetIdFromToken();
            return new ResponseDto { Status = (int)messages.Success, Message = messages.Success.GetEnumDescription(), Result = await _itemManager.GetFurtherNotifyTasks(userId) };
        }
        [HttpPut("modify-notification-status")]
        public  async Task<ResponseDto> updateNotificationStatus()
        {
            int userId = HttpContext.GetIdFromToken();
            await _itemManager.updateNotificationStatus(userId);
            return new ResponseDto { Status = (int)messages.Success, Message = messages.Success.GetEnumDescription()};
        }
        [HttpPost("cancel-notifications")]
        public async Task<ResponseDto> CancelNotifications(int[] ids)
        {
            int userId = HttpContext.GetIdFromToken();
            await _itemManager.CancelNotifications(userId,ids);
            return new ResponseDto { Status = (int)messages.Success, Message = messages.Success.GetEnumDescription() };
        }

    }
}
