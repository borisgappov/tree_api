using TreeApi.Data.Repositories;

namespace TreeApi.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ITreeRepository Trees { get; }
        INodeRepository Nodes { get; }
        IExceptionJournalRepository ExceptionJournals { get; }
        IPartnerRepository Partners { get; }
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
    
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TreeApiDbContext _context;
        private ITreeRepository? _treeRepository;
        private INodeRepository? _nodeRepository;
        private IExceptionJournalRepository? _exceptionJournalRepository;
        private IPartnerRepository? _partnerRepository;
        private bool _disposed = false;
        
        public UnitOfWork(TreeApiDbContext context)
        {
            _context = context;
        }
        
        public ITreeRepository Trees => 
            _treeRepository ??= new TreeRepository(_context);
        
        public INodeRepository Nodes => 
            _nodeRepository ??= new NodeRepository(_context);
        
        public IExceptionJournalRepository ExceptionJournals => 
            _exceptionJournalRepository ??= new ExceptionJournalRepository(_context);
        
        public IPartnerRepository Partners => 
            _partnerRepository ??= new PartnerRepository(_context);
        
        /// <summary>
        /// Saves all changes made in the current unit of work to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        /// <summary>
        /// Begins a new database transaction
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }
        
        /// <summary>
        /// Commits the current database transaction
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }
        
        /// <summary>
        /// Rolls back the current database transaction
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
        
        /// <summary>
        /// Disposes the unit of work and releases resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Disposes the unit of work and releases managed resources
        /// </summary>
        /// <param name="disposing">True if disposing managed resources, false otherwise</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
