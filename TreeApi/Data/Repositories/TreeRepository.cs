using Microsoft.EntityFrameworkCore;
using TreeApi.Data.Entities;

namespace TreeApi.Data.Repositories
{
    public class TreeRepository(TreeApiDbContext context) : Repository<Tree>(context), ITreeRepository
    {
        
        /// <summary>
        /// Retrieves a tree by its ID with all associated nodes and their children
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree</param>
        /// <returns>The tree with nodes if found, null otherwise</returns>
        public async Task<Tree?> GetTreeWithNodesAsync(long treeId)
        {
            return await _dbSet
                .Include(t => t.Nodes)
                .ThenInclude(n => n.Children)
                .FirstOrDefaultAsync(t => t.Id == treeId);
        }
        
        /// <summary>
        /// Retrieves all trees with their associated nodes and children
        /// </summary>
        /// <returns>Collection of trees with their nodes</returns>
        public async Task<IEnumerable<Tree>> GetAllTreesWithNodesAsync()
        {
            return await _dbSet
                .Include(t => t.Nodes)
                .ThenInclude(n => n.Children)
                .ToListAsync();
        }
        
        /// <summary>
        /// Retrieves a tree by its name
        /// </summary>
        /// <param name="treeName">The name of the tree</param>
        /// <returns>The tree if found, null otherwise</returns>
        public async Task<Tree?> GetTreeByNameAsync(string treeName)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Name == treeName);
        }
        
        /// <summary>
        /// Retrieves a tree by its name with all associated nodes and their children
        /// </summary>
        /// <param name="treeName">The name of the tree</param>
        /// <returns>The tree with nodes if found, null otherwise</returns>
        public async Task<Tree?> GetTreeByNameWithNodesAsync(string treeName)
        {
            return await _dbSet
                .Include(t => t.Nodes)
                .ThenInclude(n => n.Children)
                .FirstOrDefaultAsync(t => t.Name == treeName);
        }
    }
}
