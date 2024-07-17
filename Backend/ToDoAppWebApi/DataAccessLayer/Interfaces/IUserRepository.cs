using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(User user);
        public Task<User> GetByUsername(string username);
        public Task<User> AuthenticateUser(User user);
    }
}
