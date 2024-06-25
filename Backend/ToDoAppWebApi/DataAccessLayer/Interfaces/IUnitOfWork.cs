using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        IItemsRepository ItemRepository { get; }
        IUserRepository UserRepository { get; }
        IUserItemsRepository UserItemRepository { get; }
        IStatusRepository StatusRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        void SaveChanges();
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
