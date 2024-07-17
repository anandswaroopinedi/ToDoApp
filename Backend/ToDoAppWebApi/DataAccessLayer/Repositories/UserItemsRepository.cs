using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using statusEnum = Common.Enums.Status;

namespace DataAccessLayer.Repositories
{
    public class UserItemsRepository : IUserItemsRepository
    {
        private readonly ToDoAppContext _context;
        public UserItemsRepository(ToDoAppContext toDoAppContext)
        {
            _context = toDoAppContext;
        }

        public async Task<int> checkItemLinkingExists(UserItem item)
        {
            return _context.UserItems.Where(r => r.UserId == item.UserId && r.ItemId == item.ItemId && r.Status.Name.ToUpper() == statusEnum.active.ToString()).Select(r => r.Id).FirstOrDefault();
        }
        public async Task<UserItem> checkItemCompleted(UserItem item)
        {
            return _context.UserItems.Where(r => r.UserId == item.Id && r.ItemId == item.ItemId && r.Status.Name.ToUpper() == statusEnum.completed.ToString() && r.UserId == item.UserId).FirstOrDefault();
        }
        public async Task<int> AddItem(UserItem item)
        {
            _context.UserItems.Add(item);
            if (!_context.UserItems.Any())
            {
                return 0;
            }
            return _context.UserItems.Select(r => r.Id).Max();
        }
        public async Task Update(UserItem item)
        {
            _context.UserItems.Update(item);
        }
        public async Task<List<UserItem>> GetAll(int userId)
        {
            return _context.UserItems.Where(x => x.IsDeleted == 0 && x.UserId == userId && x.CreatedOn > DateTime.Today && x.CreatedOn < DateTime.Today.AddDays(1)).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
        }

        public async Task<UserItem> GetItemById(int Id, int userId)
        {
            return _context.UserItems.Where(r => r.Id == Id && r.UserId == userId).First();
        }
        public async Task DeleteItem(UserItem item, int UserId)
        {
            item.IsDeleted = 1;
            _context.UserItems.Update(item);
        }
        public async Task DeleteAllItems(int UserId)
        {

            _context.UserItems.Where(x => x.UserId == UserId && x.CreatedOn > DateTime.Today && x.CreatedOn < DateTime.Today.AddDays(1)).ToList().ForEach(x => { x.IsDeleted = 1; });
        }
        public async Task<List<UserItem>> GetActiveItems(int UserId)
        {
            return _context.UserItems.Where(x => x.Status.Name.ToUpper() == statusEnum.active.ToString() && x.IsDeleted == 0 && x.UserId == UserId && x.CreatedOn > DateTime.Today && x.CreatedOn < DateTime.Today.AddDays(1)).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
        }
        public async Task<List<UserItem>> GetCompletedItems(int UserId)
        {
            return _context.UserItems.Where(x => x.Status.Name.ToUpper() == statusEnum.completed.ToString() && x.IsDeleted == 0 && x.UserId == UserId && x.CreatedOn > DateTime.Today && x.CreatedOn < DateTime.Today.AddDays(1)).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();

        }
        public async Task<int> GetCompletedItemsCount(int userId)
        {
            return _context.UserItems.Where(u => u.Status.Name.ToUpper() == statusEnum.completed.ToString() && u.IsDeleted == 0 && u.UserId == userId && u.CreatedOn > DateTime.Today && u.CreatedOn < DateTime.Today.AddDays(1)).Count();
        }
        public async Task<int> TotalItemsCount(int userId)
        {
            return _context.UserItems.Where(u => u.IsDeleted == 0 && u.UserId == userId && u.CreatedOn > DateTime.Today && u.CreatedOn < DateTime.Today.AddDays(1)).Count();
        }
        public async Task<List<UserItem>> GetPendingTasks(int userId, string property, string order)
        {
            if (!string.IsNullOrEmpty(property))
            {
                List<UserItem> userItems = _context.UserItems
                        .Where(u => u.Status.Name.ToUpper() == statusEnum.active.ToString() &&
                        u.IsDeleted == 0 &&
                        u.UserId == userId &&
                        u.CreatedOn < DateTime.Today).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
                if (property.Equals("createdon", StringComparison.OrdinalIgnoreCase))
                {
                    if (order.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        return userItems.OrderByDescending(e => e.CreatedOn).ToList();
                    }
                    else
                    {
                        return userItems.OrderBy(e => e.CreatedOn).ToList();
                    }
                }
                else
                {
                    if (order.Equals("asc", StringComparison.OrdinalIgnoreCase))
                    {
                        return userItems.OrderBy(e => e.Item.Name).ToList();
                    }
                    else
                    {
                        return userItems.OrderByDescending(e => e.Item.Name).ToList();
                    }
                }

            }
            return [];

        }
        public async Task<List<UserItem>> GetNotifyTasks(int userId)
        {
            return _context.UserItems.Where(u => u.Status.Name.ToUpper() == statusEnum.active.ToString() &&
                        u.IsDeleted == 0 &&
                        u.UserId == userId && (u.NotifyOn <= DateTime.Now) && u.IsNotifyCancelled == 0).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).OrderByDescending(u=>u.CreatedOn).Take(10).ToList();
        }
        public async Task<List<UserItem>> GetFurtherNotifyTasks(int userId)
        {
            return _context.UserItems.Where(u => u.Status.Name.ToUpper() == statusEnum.active.ToString() &&
                        u.IsDeleted == 0 &&
                        u.UserId == userId && (u.NotifyOn >= DateTime.Now) && (u.NotifyOn <= DateTime.Now.AddDays(1)) && u.IsNotifyCancelled == 0).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();

        }
        public async Task CancelNotifications(int userId,int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                UserItem u = _context.UserItems.Where(u =>u.UserId==userId && u.Id == ids[i]).FirstOrDefault();
                u.IsNotifyCancelled = 1;
                _context.UserItems.Update(u);
            }
        }
        public async Task updateNotificationStatus(int userId)
        {
            List<UserItem> items = _context.UserItems.Where(u => u.IsNotifyCancelled == 1 && u.Status.Name.ToUpper() == statusEnum.active.ToString()).ToList();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].IsNotifyCancelled = 0;
                _context.Update(items[i]);
            }
        }
    }
}
