using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IItemsRepository
    {
        public Task Add(Item item);
        public Task<int> checkItemExists(Item item);
        public Task<int> recentlyAddedId();

    }
}
