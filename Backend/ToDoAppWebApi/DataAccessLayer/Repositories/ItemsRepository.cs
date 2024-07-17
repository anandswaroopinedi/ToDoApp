using DataAccessLayer.Interfaces;
using System.Data;
using DataAccessLayer.Entities;
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
    }
}
