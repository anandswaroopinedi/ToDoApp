using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly ToDoAppContext _toDoAppContext;
        public ErrorLogRepository(ToDoAppContext toDoAppContext)
        {
            _toDoAppContext = toDoAppContext;
        }
        public async Task AddError(ErrorLog errorlog)
        {
            _toDoAppContext.ErrorLogs.Add(errorlog);
        }
    }
}
