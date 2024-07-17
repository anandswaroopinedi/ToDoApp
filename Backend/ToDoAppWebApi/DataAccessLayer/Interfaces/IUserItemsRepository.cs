using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUserItemsRepository
    {
        public Task<UserItem> checkItemCompleted(UserItem item);
        public Task<int> checkItemLinkingExists(UserItem item);
        public Task Update(UserItem item);
        public Task<int> AddItem(UserItem item);
        public Task<List<UserItem>> GetAll(int userId);
        public Task<UserItem> GetItemById(int id, int userId);
        public Task DeleteItem(UserItem item, int UserId);
        public Task DeleteAllItems(int UserId);
        public Task<List<UserItem>> GetActiveItems(int UserId);
        public Task<List<UserItem>> GetCompletedItems(int UserId);
        public Task<int> GetCompletedItemsCount(int userId);
        public Task<int> TotalItemsCount(int userId);
        public Task<List<UserItem>> GetPendingTasks(int userId, string property, string order);
        public Task<List<UserItem>> GetNotifyTasks(int userId);
        public Task<List<UserItem>> GetFurtherNotifyTasks(int userId);
        public Task updateNotificationStatus(int userId);
        public Task CancelNotifications(int userId, int[] ids);
    }
}
