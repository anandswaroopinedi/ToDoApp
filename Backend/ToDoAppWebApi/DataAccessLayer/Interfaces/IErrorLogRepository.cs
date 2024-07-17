using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IErrorLogRepository
    {
        public Task AddError(ErrorLog errorlog);
    }
}
