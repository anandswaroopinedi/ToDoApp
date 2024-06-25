using AutoMapper;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Models;

namespace BusinessLogicLayer.Services
{
    public class ItemManager : IItemManager
    {
        private readonly IItemsRepository _taskRepository;
        private readonly IUserItemsRepository _userItemsRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IHttpContextAccessor  _httpContextAccessor;
        public ItemManager(IItemsRepository taskRepository, IUserItemsRepository usersItemsRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _userItemsRepository = usersItemsRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddItem(ItemDto item)
        {
            _unitOfWork.BeginTransaction();
            string time = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            int statusId = await _unitOfWork.StatusRepository.getIdByName("ACTIVE");
            int result = await _unitOfWork.ItemRepository.checkItemExists(_mapper.Map<Item>(item));
            if (result > 0)
            {

                item.Itemid = result;
                result = await _unitOfWork.UserItemRepository.checkItemLinkingExists(_mapper.Map<UserItem>(item));
                if (result > 0)
                {
                    return 2;
                }
                UserItem u = await _unitOfWork.UserItemRepository.checkItemCompleted(_mapper.Map<UserItem>(item));
                if (u != null)
                {
                    u.StatusId = statusId;
                    u.CreatedOn = time;
                    await _unitOfWork.UserItemRepository.Update(u);
                }
                else
                {
                    item.Statusid = statusId;
                    item.Createdon = time;
                    await _unitOfWork.UserItemRepository.AddItem(_mapper.Map<UserItem>(item));
                }

            }
            else
            {
                await _unitOfWork.ItemRepository.Add(_mapper.Map<Item>(item));
                int id = await _unitOfWork.ItemRepository.recentlyAddedId();
                item.Itemid = id + 1;
                item.Createdon = time;
                item.Statusid = statusId;
                await _unitOfWork.UserItemRepository.AddItem(_mapper.Map<UserItem>(item));
            }
            _unitOfWork.Commit();
            return 1;
        }
        public async Task<List<ItemDto>> GetAll(int userId)
        {

            return _mapper.Map<List<ItemDto>>(await _unitOfWork.UserItemRepository.GetAll(userId));
        }
        public async Task UpdateItem(ItemDto item)
        {
            _unitOfWork.BeginTransaction();
            int result = await _unitOfWork.ItemRepository.checkItemExists(_mapper.Map<Item>(item));
            if (result == 0)
            {
                await _unitOfWork.ItemRepository.Add(_mapper.Map<Item>(item));
                item.Itemid = await _unitOfWork.ItemRepository.recentlyAddedId() + 1;
            }
            else
            {
                item.Itemid = result;
            }
            item.Isdeleted = 0;
            await _unitOfWork.UserItemRepository.Update(_mapper.Map<UserItem>(item));
            _unitOfWork.Commit();
        }
        public async Task<int> DeleteItem(int id, int userId)
        {
            UserItem result = await _unitOfWork.UserItemRepository.GetItemById(id, userId);
            if (result != null)
            {
                await _unitOfWork.UserItemRepository.DeleteItem(result, userId);
                _unitOfWork.Commit();
                return 1;
            }
            return 2;
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
        public async Task<int> makeItemCompleted(int id, int userId)
        {
            UserItem result = await _unitOfWork.UserItemRepository.GetItemById(id, userId);
            if (result != null)
            {
                result.StatusId = await _unitOfWork.StatusRepository.getIdByName("COMPLETED");
                result.CompletedOn = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                await _unitOfWork.UserItemRepository.Update(result);
                _unitOfWork.Commit();
                return 1;

            }
            else
            {
                return 3;

            }
        }
        public async Task<int> makeItemActive(int id, int userId)
        {
            UserItem result = await _unitOfWork.UserItemRepository.GetItemById(id, userId);
            if (result != null)
            {
                result.StatusId = await _unitOfWork.StatusRepository.getIdByName("ACTIVE");
                result.CreatedOn = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                await _unitOfWork.UserItemRepository.Update(result);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return 1;
            }
            else
            {
                return 3;
            }

        }
        /*public async Task<ApiResponse> AddItem(ItemDto item)
        {
            //item.Userid=_httpContextAccessor.HttpContext.User.FindFirst<
            ApiResponse response = await _taskRepository.AddItem(_mapper.Map<Item>(item));
            if (response != null && response.StatusCode == 200)
            {
                item.Itemid = (int)response.result;
                return await _userItemsRepository.AddItem(_mapper.Map<Useritem>(item));
            }
            else
            {
                {
                    return response;
                }
            }
        }
        public async Task<ApiResponse> GetAll(int userId)
        {
            ApiResponse apiResponse = await _userItemsRepository.GetAllItems(userId);
            apiResponse.result = _mapper.Map<List<ItemDto>>(apiResponse.result);
            return apiResponse;
        }
        public async Task<ApiResponse> UpdateItem(ItemDto item)
        {
            ApiResponse response = await _taskRepository.GetId(_mapper.Map<Item>(item));
            if (response.StatusCode == 200)
            {
                item.Itemid = (int)response.result;
                return await _userItemsRepository.UpdateItem(_mapper.Map<Useritem>(item));
            }
            else
            {
                return response;
            }

        }
        public async Task<ApiResponse> DeleteItem(int id, int userId)
        {
            return await _userItemsRepository.DeleteItem(id, userId);
        }
        public async Task<ApiResponse> DeleteItems(int userId)
        {
            return await _userItemsRepository.DeleteItems(userId);
        }
        public async Task<ApiResponse> GetActiveItems(int userId)
        {
            //int userId = ClaimsIdentifier.getIdFromToken(HttpContext);
            ApiResponse apiResponse = await _userItemsRepository.GetActiveItems(userId);
            apiResponse.result = _mapper.Map<List<ItemDto>>(apiResponse.result);
            return apiResponse;
        }
        public async Task<ApiResponse> GetCompletedItems(int userId)
        {
            ApiResponse apiResponse = await _userItemsRepository.GetCompletedItems(userId);
            apiResponse.result = _mapper.Map<List<ItemDto>>(apiResponse.result);
            return apiResponse;
        }
        public async Task<ApiResponse> CompletionPercentage(int userId)
        {
            return await _userItemsRepository.CompletionPercentage(userId);
        }
        public async Task<ApiResponse> makeItemCompleted(int id, int userId)
        {
            return await _userItemsRepository.makeItemCompleted(id, userId);
        }
        public async Task<ApiResponse> makeItemActive(int id, int UserId)
        {
            return await _userItemsRepository.makeItemActive(id, UserId);
        }*/
    }
}
