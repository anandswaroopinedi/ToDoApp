using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Models;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;

namespace DataAccessLayer.Repositories
{
    public class UserItemsRepository:IUserItemsRepository
    {
        private readonly ToDoAppContext _context;
        private static string todayString = DateTime.Today.ToString("MM/dd/yyyy");
        public UserItemsRepository(ToDoAppContext toDoAppContext)
        {
            _context = toDoAppContext;
        }

        public async Task<int> checkItemLinkingExists(UserItem item)
        {
            return _context.UserItems.Where(r => r.UserId == item.UserId && r.ItemId == item.ItemId && r.Status.Name.ToUpper() == "ACTIVE" ).Select(r => r.Id).FirstOrDefault();
        }
        public async Task<UserItem> checkItemCompleted(UserItem item)
        {
            return _context.UserItems.Where(r => r.UserId == item.Id && r.ItemId == item.ItemId && r.Status.Name.ToUpper() == "COMPLETED" && r.UserId == item.UserId).FirstOrDefault();
        }
        public async Task AddItem(UserItem item)
        {
              _context.UserItems.Add(item);
        }
        public async Task Update(UserItem item)
        {
            _context.UserItems.Update(item);
        }
        public async Task<List<UserItem>> GetAll(int userId)
        {
           return _context.UserItems.Where(x => x.IsDeleted == 0 && x.UserId == userId && EF.Functions.Like(x.CreatedOn, todayString + "%")).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
        }

        public async Task<UserItem> GetItemById(int Id,int userId)
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

           _context.UserItems.Where(x => x.UserId == UserId).ToList().ForEach(x => { x.IsDeleted = 1; });
       }
        public async Task<List<UserItem>> GetActiveItems(int UserId)
        {
            return _context.UserItems.Where(x => x.Status.Name.ToUpper() == "ACTIVE" && x.IsDeleted == 0 && x.UserId == UserId && EF.Functions.Like(x.CreatedOn, todayString + "%")).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
        }
        public async Task<List<UserItem>> GetCompletedItems(int UserId)
        {
            return _context.UserItems.Where(x => x.Status.Name.ToUpper() == "COMPLETED" && x.IsDeleted == 0 && x.UserId == UserId && EF.Functions.Like(x.CreatedOn, todayString + "%")).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
            
        }
        public async Task<int> GetCompletedItemsCount(int userId)
        {
            return _context.UserItems.Where(u => u.Status.Name.ToUpper() == "COMPLETED" && u.IsDeleted == 0 && u.UserId == userId && EF.Functions.Like(u.CreatedOn, todayString + "%")).Count();
        }
        public async Task<int> TotalItemsCount(int userId)
        {
            return _context.UserItems.Where(u => u.IsDeleted == 0 && u.UserId == userId && EF.Functions.Like(u.CreatedOn, todayString + "%")).Count();
        }

        /* public async Task<ApiResponse> AddItem(Useritem item)
         {
             try
             {
                 int result = _context.Useritems.Where(r => r.Userid == item.Userid && r.Itemid == item.Itemid && r.Status.Name.ToUpper() == "ACTIVE").Select(r => r.Id).FirstOrDefault();
                 if (result > 0)
                 {
                     return new ApiResponse
                     {
                         StatusCode = 400,
                         Message = "Task is already added in the ToDo List"
                     };
                 }
                 else
                 {
                     Useritem t=_context.Useritems.Where(r => r.Userid == item.Id && r.Itemid == item.Itemid && r.Status.Name.ToUpper() == "COMPLETED" && r.Userid==item.Userid).FirstOrDefault();
                     string time= DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                     int statusId = _context.Statuses.Where(s => s.Name.ToUpper() == "ACTIVE").Select(s => s.Id).First();
                     if (t != null)
                     {
                         t.Statusid = statusId;
                         t.Createdon = time;
                         _context.Useritems.Update(t);
                     }
                     else
                     {
                         item.Statusid = statusId;
                         item.Createdon = time;
                         _context.Useritems.Add(item);
                     }
                     _context.SaveChanges();
                     return new ApiResponse
                     {
                         StatusCode = 200,
                         Message = "Successfully added the task to the user ToDo"
                     };
                 }
             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> CompletionPercentage(int UserId)
         {
             try
             {
                 int count = _context.Useritems.Where(u => u.Status.Name.ToUpper() == "COMPLETED" && u.Isdeleted == 0 && u.Userid == UserId && EF.Functions.Like(u.Createdon, todayString + "%")).Count();
                 int totalCount = _context.Useritems.Where(u => u.Isdeleted == 0 && u.Userid == UserId && EF.Functions.Like(u.Createdon, todayString + "%")).Count();
                 int completedPercentage = 0;
                 int activePercentage = 0;
                 if (totalCount > 0)
                 {
                      completedPercentage = (int)Math.Round((double)count*100 / totalCount);
                      activePercentage = (int)Math.Round((double)(totalCount-count) * 100 / totalCount);
                 }
                 return new ApiResponse { StatusCode = 200, Message = "Successful", result = new int[] { completedPercentage, activePercentage } };
             }
             catch(Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> makeItemCompleted(int id, int UserId)
         {
             try
             {
                 Useritem result = _context.Useritems.Where(r => r.Id == id && r.Userid == UserId).First();
                 if (result != null)
                 {
                     result.Statusid = _context.Statuses.Where(s => s.Name.ToUpper() == "COMPLETED" && s.Isdeleted==0).Select(s => s.Id).First();
                     result.Completedon= DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                     _context.Useritems.Update(result);
                     _context.SaveChanges();
                     return new ApiResponse
                     {
                         StatusCode = 200,
                         Message = "Task Updated as completed"
                     };
                 }
                 else
                 {
                     return new ApiResponse
                     {
                         StatusCode = 500,
                         Message = "Task not found"
                     };
                 }

             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> makeItemActive(int id, int UserId)
         {
             try
             {
                 Useritem result = _context.Useritems.Where(r => r.Id == id && r.Userid == UserId).First();
                 if (result != null)
                 {
                     result.Statusid = _context.Statuses.Where(s => s.Name.ToUpper() == "ACTIVE" && s.Isdeleted == 0).Select(s => s.Id).First();
                     result.Createdon = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                     result.Completedon = null;
                     _context.Useritems.Update(result);
                     _context.SaveChanges();
                     return new ApiResponse
                     {
                         StatusCode = 200,
                         Message = "Task Updated as active"
                     };
                 }
                 else
                 {
                     return new ApiResponse
                     {
                         StatusCode = 500,
                         Message = "Task not found"
                     };
                 }

             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> DeleteItem(int id,int UserId)
         {
             try
             {
                 Useritem result = _context.Useritems.Where(r => r.Id == id && r.Userid== UserId).First();
                 if (result != null)
                 {
                     result.Isdeleted = 1;
                     _context.Useritems.Update(result);
                     _context.SaveChanges();
                     return new ApiResponse
                     {
                         StatusCode = 200,
                         Message = "Task Deleted from the list successfully"
                     };
                 }
                 else
                 {
                     return new ApiResponse
                     {
                         StatusCode = 500,
                         Message = "Id not found"
                     };
                 }

             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> GetAllItems(int UserId)
         {
             try
             {

                 List<Useritem> Useritems = _context.Useritems.Where(x => x.Isdeleted == 0 && x.Userid == UserId  && EF.Functions.Like(x.Createdon, todayString + "%")).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList();
                 return new ApiResponse
                 {
                     StatusCode = 200,
                     Message = "Retrieval successful",
                     result = Useritems
                 };
             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> UpdateItem(Useritem useritem)
         {
             try
             {
                 int id = _context.Useritems.Where(u => u.Id == useritem.Id && u.Userid == useritem.Userid).Select(u=>u.Id).FirstOrDefault();
                 if (id != 0)
                 {
                     useritem.Createdon=DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                     useritem.Isdeleted = 0;
                     _context.Useritems.Update(useritem);
                     _context.SaveChanges();
                     return new ApiResponse
                     {
                         StatusCode = 200,
                         Message = "Updated successfully"
                     };
                 }
                 else
                 {
                     return new ApiResponse
                     {
                         StatusCode = 500,
                         Message = "Updation unsuccessful because Usertask doesn't exist in database"
                     };
                 }
             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> GetActiveItems(int UserId)
         {
             try
             {
                 return new ApiResponse 
                         {
                             StatusCode=200,
                             Message="Successful",
                             result = _context.Useritems.Where(x=>x.Status.Name.ToUpper()=="ACTIVE" && x.Isdeleted == 0 && x.Userid == UserId && EF.Functions.Like(x.Createdon, todayString + "%")).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList()
                         };

             }
             catch(Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> GetCompletedItems(int UserId)
         {
             try
             {
                 return new ApiResponse
                 {
                     StatusCode = 200,
                     Message = "Successful",
                     result = _context.Useritems.Where(x => x.Status.Name.ToUpper() == "COMPLETED" && x.Isdeleted==0 && x.Userid==UserId && EF.Functions.Like(x.Createdon, todayString + "%")).Include(u => u.Item).Include(u => u.User).Include(u => u.Status).ToList()
                 };

             }
             catch (Exception ex)
             {
                 return new ApiResponse
                 {
                     StatusCode = 500,
                     Message = ex.Message
                 };
             }
         }
         public async Task<ApiResponse> DeleteItems(int UserId)
         {
             try
             {
                 _context.Useritems.Where(x=>x.Userid==UserId).ToList().ForEach(x => { x.Isdeleted=1; });
                 _context.SaveChanges();
                 return new ApiResponse { StatusCode = 200, Message = "Successfully Deleted" };
             }
             catch(Exception ex)
             {
                 return new ApiResponse { StatusCode = 500,Message=ex.Message };
             }

         }*/
    }
}
