using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ToDoAppContext _context;
        private IItemsRepository _itemRepository;
        private IUserRepository _userRepository;
        private IUserItemsRepository _userItemRepository;
        private IDbContextTransaction _transaction;
        private IStatusRepository _statusRepository;
        private IErrorLogRepository _errorRepository;
        public UnitOfWork(ToDoAppContext context)
        {
            _context = context;
            _transaction = null;
        }

        public IItemsRepository ItemRepository
        {
            get { return _itemRepository ??= new ItemsRepository(_context); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IUserItemsRepository UserItemRepository
        {
            get { return _userItemRepository ??= new UserItemsRepository(_context); }
        }
        public IStatusRepository StatusRepository
        {
            get { return _statusRepository ??= new StatusRepository(_context); }
        }
        public IErrorLogRepository ErrorLogRepository
        {
            get { return _errorRepository ??= new ErrorLogRepository(_context); }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
                _transaction?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }
    }
}
