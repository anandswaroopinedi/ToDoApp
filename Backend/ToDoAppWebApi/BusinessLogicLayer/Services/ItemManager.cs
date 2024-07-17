using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Models.DtoModels;
using EntityItem = DataAccessLayer.Entities.Item;
using EntityUserItem = DataAccessLayer.Entities.UserItem;
using InputItem = Models.InputModels.Item;

namespace BusinessLogicLayer.Services
{
    public class ItemManager : IItemManager
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ItemManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddItem(InputItem item)
        {
            _unitOfWork.BeginTransaction();
            int statusId = await _unitOfWork.StatusRepository.getIdByName("ACTIVE");
            int result = await _unitOfWork.ItemRepository.checkItemExists(_mapper.Map<EntityItem>(_mapper.Map<ItemDto>(item)));
            int id = 0;
            if(item.NotifyOn==null)
            {
                item.NotifyOn = DateTime.Today.AddDays(1);
            }
            if (result > 0)
            {

                item.Itemid = result;
                result = await _unitOfWork.UserItemRepository.checkItemLinkingExists(_mapper.Map<EntityUserItem>(_mapper.Map<ItemDto>(item)));
                if (result > 0)
                {
                    return 0;
                }
                EntityUserItem u = await _unitOfWork.UserItemRepository.checkItemCompleted(_mapper.Map<EntityUserItem>(_mapper.Map<ItemDto>(item)));
                if (u != null)
                {
                    u.StatusId = statusId;
                    await _unitOfWork.UserItemRepository.Update(u);
                }
                else
                {
                    item.Statusid = statusId;
                    id = await _unitOfWork.UserItemRepository.AddItem(_mapper.Map<EntityUserItem>(_mapper.Map<ItemDto>(item)));
                }

            }
            else
            {
                await _unitOfWork.ItemRepository.Add(_mapper.Map<EntityItem>(_mapper.Map<ItemDto>(item)));
                int itemId = await _unitOfWork.ItemRepository.recentlyAddedId();
                item.Itemid = itemId + 1;
                item.Statusid = statusId;
                id=await _unitOfWork.UserItemRepository.AddItem(_mapper.Map<EntityUserItem>(_mapper.Map<ItemDto>(item)));
            }
            _unitOfWork.Commit();
            return id+1;
        }
        public async Task<List<ItemDto>> GetAll(int userId)
        {

            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetAll(userId));
        }
        public async Task UpdateItem(InputItem item)
        {
            _unitOfWork.BeginTransaction();
            int result = await _unitOfWork.ItemRepository.checkItemExists(_mapper.Map<EntityItem>(_mapper.Map<ItemDto>(item)));
            if (result == 0)
            {
                await _unitOfWork.ItemRepository.Add(_mapper.Map<EntityItem>(_mapper.Map<ItemDto>(item)));
                item.Itemid = await _unitOfWork.ItemRepository.recentlyAddedId() + 1;
            }
            else
            {
                item.Itemid = result;
            }
            item.Isdeleted = 0;
            await _unitOfWork.UserItemRepository.Update(_mapper.Map<EntityUserItem>(_mapper.Map<ItemDto>(item)));
            _unitOfWork.Commit();
        }
        public async Task<bool> DeleteItem(int id, int userId)
        {
            EntityUserItem result = await _unitOfWork.UserItemRepository.GetItemById(id, userId);
            if (result != null)
            {
                await _unitOfWork.UserItemRepository.DeleteItem(result, userId);
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }
        public async Task DeleteItems(int userId)
        {
            await _unitOfWork.UserItemRepository.DeleteAllItems(userId);
            _unitOfWork.Commit();
        }
        public async Task<List<ItemDto>> GetActiveItems(int userId)
        {
            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetActiveItems(userId));
        }
        public async Task<List<ItemDto>> GetCompletedItems(int userId)
        {
            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetCompletedItems(userId));
        }
        public async Task<int[]> CompletionPercentage(int userId)
        {
            int count = await _unitOfWork.UserItemRepository.GetCompletedItemsCount(userId);
            int totalCount = await _unitOfWork.UserItemRepository.TotalItemsCount(userId);
            int completedPercentage = 0;
            int activePercentage = 0;
            if (totalCount > 0)
            {
                completedPercentage = (int)Math.Round((double)count * 100 / totalCount);
                activePercentage = (int)Math.Round((double)(totalCount - count) * 100 / totalCount);
            }
            return new int[] { completedPercentage, activePercentage };
        }
        public async Task<bool> makeItemCompleted(int id, int userId)
        {
            EntityUserItem result = await _unitOfWork.UserItemRepository.GetItemById(id, userId);
            if (result != null)
            {
                result.StatusId = await _unitOfWork.StatusRepository.getIdByName("COMPLETED");
                await _unitOfWork.UserItemRepository.Update(result);
                _unitOfWork.Commit();
                return true;

            }
            else
            {
                return false;

            }
        }
        public async Task<bool> makeItemActive(int id, int userId)
        {
            EntityUserItem result = await _unitOfWork.UserItemRepository.GetItemById(id, userId);
            if (result != null)
            {
                result.StatusId = await _unitOfWork.StatusRepository.getIdByName("ACTIVE");
                await _unitOfWork.UserItemRepository.Update(result);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<List<ItemDto>> GetPendingTasks(int userId, string property, string order)
        {
            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetPendingTasks(userId, property, order));
        }
        public async Task<List<ItemDto>> GetNotifyTasks(int userId)
        {
            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetNotifyTasks(userId));
        }
        public async Task<List<ItemDto>> GetFurtherNotifyTasks(int userId)
        {
            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetFurtherNotifyTasks(userId));
        }
        public async Task updateNotificationStatus(int userId)
        {
            _unitOfWork.BeginTransaction();
           await _unitOfWork.UserItemRepository.updateNotificationStatus(userId);
            _unitOfWork.Commit();
        }
        public async Task CancelNotifications(int userId, int[] ids)
        {
            _unitOfWork.BeginTransaction();
            await _unitOfWork.UserItemRepository.CancelNotifications(userId, ids);
            _unitOfWork.Commit();
        }
    }
}
