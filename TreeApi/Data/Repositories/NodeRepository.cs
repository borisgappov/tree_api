using Microsoft.EntityFrameworkCore;
using TreeApi.Data.Entities;

namespace TreeApi.Data.Repositories
{
    public class NodeRepository(TreeApiDbContext context) : Repository<Node>(context), INodeRepository
    {
        
        /// <summary>
        /// Retrieves all nodes for a specific tree with their children
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree</param>
        /// <returns>Collection of nodes belonging to the specified tree</returns>
        public async Task<IEnumerable<Node>> GetNodesByTreeIdAsync(long treeId)
        {
            return await _dbSet
                .Include(n => n.Children)
                .Where(n => n.TreeId == treeId)
                .ToListAsync();
        }
        
        /// <summary>
        /// Retrieves a node by its ID with all its children and tree information
        /// </summary>
        /// <param name="nodeId">The unique identifier of the node</param>
        /// <returns>The node with children and tree if found, null otherwise</returns>
        public async Task<Node?> GetNodeWithChildrenAsync(long nodeId)
        {
            return await _dbSet
                .Include(n => n.Children)
                .Include(n => n.Tree)
                .FirstOrDefaultAsync(n => n.Id == nodeId);
        }
        
        /// <summary>
        /// Retrieves all root nodes (nodes without parent) for a specific tree
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree</param>
        /// <returns>Collection of root nodes belonging to the specified tree</returns>
        public async Task<IEnumerable<Node>> GetRootNodesByTreeIdAsync(long treeId)
        {
            return await _dbSet
                .Include(n => n.Children)
                .Where(n => n.TreeId == treeId && n.ParentId == null)
                .ToListAsync();
        }
    }
}
