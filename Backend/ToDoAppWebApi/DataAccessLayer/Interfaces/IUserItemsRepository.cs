using DataAccessLayer.Entities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserItemsRepository
    {
        public Task<UserItem> checkItemCompleted(UserItem item);
        public Task<int> checkItemLinkingExists(UserItem item);
        public Task Update(UserItem item);
        public Task AddItem(UserItem item);
        public Task<List<UserItem>> GetAll(int userId);
        public Task<UserItem> GetItemById(int id, int userId);
        public Task DeleteItem(UserItem item, int UserId);
        public Task DeleteAllItems(int UserId);
        public Task<List<UserItem>> GetActiveItems(int UserId);
        public Task<List<UserItem>> GetCompletedItems(int UserId);
        public Task<int> GetCompletedItemsCount(int userId);
        public Task<int> TotalItemsCount(int userId);

        /*public Task<ApiResponse> AddItem(UserItem item);
        public Task<ApiResponse> DeleteItem(int id, int UserId);
        public Task<ApiResponse> GetAllItems(int UserId);
        public Task<ApiResponse> UpdateItem(UserItem item);
        public Task<ApiResponse> GetActiveItems(int UserId);
        public Task<ApiResponse> GetCompletedItems(int UserId);
        public Task<ApiResponse> DeleteItems(int UserId);
        public Task<ApiResponse> CompletionPercentage(int UserId);
        public  Task<ApiResponse> makeItemCompleted(int id, int UserId);
        public Task<ApiResponse> makeItemActive(int id, int UserId);*/

    }
}
