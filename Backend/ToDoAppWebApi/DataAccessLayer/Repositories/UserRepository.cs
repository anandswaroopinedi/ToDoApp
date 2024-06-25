using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ToDoAppContext _context;
        public UserRepository(ToDoAppContext toDoAppContext) {
            _context = toDoAppContext;
        }
        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
        }
        /*public async Task<ApiResponse> AddUser(User user)
        {
            try
            {
                User result = _context.Users.Where(u => u.Username == user.Username).FirstOrDefault();
                if (result != null)
                {
                    return new ApiResponse
                    {
                        StatusCode = 500,
                        Message = "UserName Already Exists so choose other",
                        result = -1
                    };
                }
                else
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return new ApiResponse
                    {
                        StatusCode = 200,
                        Message = "Successfully registered",
                        result = 0
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

        }*/
        public async Task<User> GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }
        public async Task<User> AuthenticateUser(User user)
        {
            return _context.Users.Where(u => u.UserName.ToLower() == user.UserName.ToLower()).FirstOrDefault();
        }
        /*public async Task<ApiResponse> AuthenticateUser(User user)
        {
            try
            {
                User result = _context.Users.Where(u => u.Username == user.Username).FirstOrDefault();
                if (result != null)
                {
                    if (user.Password.Equals(result.Password, StringComparison.OrdinalIgnoreCase))
                    {
                        return new ApiResponse
                        {
                            StatusCode = 200,
                            Message = "successfully logged in",
                            result=result.Id
                        };

                    }
                    else
                    {
                        return new ApiResponse
                        {
                            StatusCode = 500,
                            Message = "Password is Incorrect",
                            result = -1
                        };
                    }
                }
                else
                {
                    return new ApiResponse
                    {
                        StatusCode = 500,
                        Message = "UserName incorrect check the username",
                        result = -1
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse
                {
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }*/
    }
}
