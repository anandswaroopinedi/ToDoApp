using DataAccessLayer.Entities;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserManager
    {
        public Task<int> AuthenticateUser(UserDto user);
        public Task<int> AddUser(UserDto user);
        public Task<string> GenerateToken(int id);
    }
}
