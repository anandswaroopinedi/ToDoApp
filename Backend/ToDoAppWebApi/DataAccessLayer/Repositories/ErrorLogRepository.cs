using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
