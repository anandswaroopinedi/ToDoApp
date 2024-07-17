using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ToDoAppContext _context;
        public UserRepository(ToDoAppContext toDoAppContext)
        {
            _context = toDoAppContext;
        }
        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
        }
        public async Task<User> GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }
        public async Task<User> AuthenticateUser(User user)
        {
            return _context.Users.Where(u => u.UserName.ToLower() == user.UserName.ToLower()).FirstOrDefault();
        }
    }
}
