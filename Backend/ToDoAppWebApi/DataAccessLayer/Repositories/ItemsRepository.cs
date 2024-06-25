using Azure;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Models;
using System.Collections.Generic;
using System.Data;
namespace DataAccessLayer.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly ToDoAppContext _toDoAppContext;
        public ItemsRepository(ToDoAppContext toDoAppContext)
        {
            _toDoAppContext = toDoAppContext;
        }
        public async Task<int> checkItemExists(Item item)
        {
            return _toDoAppContext.Items.Where(r => r.Name.ToUpper() == item.Name.ToUpper() && r.Description.ToUpper() == item.Description.ToUpper()).Select(r => r.Id).FirstOrDefault();
        }
        public async Task Add(Item item)
        {
            //check id
            item.Id = 0;
            _toDoAppContext.Add(item);
        }
        public async Task<int> recentlyAddedId()
        {
            if (!_toDoAppContext.Items.Any())
            {
                return 0;
            }
            return _toDoAppContext.Items.Select(r => r.Id).Max();
        }
        /*public async Task<ApiResponse> AddItem(Item item)
        {
            try
            {
                int result = _toDoAppContext.Items.Where(r => r.Name.ToUpper() == item.Name.ToUpper() && r.Description.ToUpper()==item.Description.ToUpper()).Select(r => r.Id).FirstOrDefault();
                if (result > 0)
                {
                    var apiResponse1 = new ApiResponse
                    {
                        StatusCode = 200,
                        Message = "Task already exists",
                        result = result
                    };
                    return apiResponse1;
                }
                _toDoAppContext.Items.Add(item);
                var apiResponse = new ApiResponse
                {
                    StatusCode = 200,
                    Message = "Task added successfully"
                };
                _toDoAppContext.SaveChanges();
                apiResponse.result = _toDoAppContext.Items.Select(r => r.Id).Max();
                return apiResponse;

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    StatusCode = 500, 
                    Message = ex.Message
                };
                return apiResponse;
            }
        }
        public async Task<ApiResponse> DeleteItem(Item item)
        {
            try
            {
                Item t = _toDoAppContext.Items.Where(t => t.Id == item.Id).FirstOrDefault();
                t.Isdeleted = 1;
                _toDoAppContext.Items.Update(t);
                _toDoAppContext.SaveChanges();
                var apiResponse = new ApiResponse
                {
                    StatusCode = 200, // or another appropriate HTTP status code
                    Message = "Task deleted successfully"
                };
                return apiResponse;
            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    StatusCode = 500, // or another appropriate HTTP status code
                    Message = ex.Message
                };
                return apiResponse;
            }
        }
        public async Task<ApiResponse> GetId(Item item)
        {
            try
            {
                int id = _toDoAppContext.Items.Where(x => x.Name == item.Name && x.Description.ToLower()==item.Description.ToLower()).Select(x => x.Id).FirstOrDefault();
                if (id == 0)
                {
                    item.Id = 0;
                    return await AddItem(item);
                }
                else
                {
                   return new ApiResponse
                            {
                                StatusCode = 200,
                                Message = "Successful",
                                result =id
                            };
                }

            }
            catch (Exception ex)
            {
                var apiResponse = new ApiResponse
                {
                    StatusCode = 500, 
                    Message = ex.Message
                };
                return apiResponse;
            }
        }
*/
    }
}
