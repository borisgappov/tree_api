using TreeApi.Data.Entities;

namespace TreeApi.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(long id);
    }
    
    public interface ITreeRepository : IRepository<Tree>
    {
        Task<Tree?> GetTreeWithNodesAsync(long treeId);
        Task<IEnumerable<Tree>> GetAllTreesWithNodesAsync();
        Task<Tree?> GetTreeByNameAsync(string treeName);
        Task<Tree?> GetTreeByNameWithNodesAsync(string treeName);
    }
    
    public interface INodeRepository : IRepository<Node>
    {
        Task<IEnumerable<Node>> GetNodesByTreeIdAsync(long treeId);
        Task<Node?> GetNodeWithChildrenAsync(long nodeId);
        Task<IEnumerable<Node>> GetRootNodesByTreeIdAsync(long treeId);
    }
    
    public interface IExceptionJournalRepository : IRepository<ExceptionJournal>
    {
        Task<IEnumerable<ExceptionJournal>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ExceptionJournal>> GetByEventIdAsync(long eventId);
    }
    
    public interface IPartnerRepository : IRepository<Partner>
    {
        Task<Partner?> GetByCodeAsync(string code);
        Task<bool> CodeExistsAsync(string code);
    }
}
