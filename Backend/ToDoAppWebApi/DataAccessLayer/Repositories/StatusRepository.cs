using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly ToDoAppContext _context;
        public StatusRepository(ToDoAppContext toDoAppContext)
        {
            _context = toDoAppContext;
        }

        public async Task<int> getIdByName(string name)
        {
            return _context.Statuses.Where(s => s.Name.ToUpper() == name.ToUpper()).Select(s => s.Id).First();
        }
    }
}
