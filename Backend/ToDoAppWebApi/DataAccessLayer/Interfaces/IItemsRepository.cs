using DataAccessLayer.Entities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IItemsRepository
    {
        public Task Add(Item item);
        public Task<int> checkItemExists(Item item);
        public Task<int> recentlyAddedId();
        /*public Task<ApiResponse> AddItem(Item item);
        public Task<ApiResponse> DeleteItem(Item item);
        public Task<ApiResponse> GetId(Item item);*/
    }
}
