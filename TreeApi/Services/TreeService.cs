using AutoMapper;
using TreeApi.Data.Entities;
using TreeApi.Data.UnitOfWork;
using TreeApi.Models;
using TreeApi.Exceptions;

namespace TreeApi.Services
{
    public interface ITreeService
    {
        Task<IEnumerable<MNode>> GetAllTreesAsync();
        Task<MNode?> GetTreeByIdAsync(long treeId);
        Task<MNode> CreateTreeAsync(MNode treeModel);
        Task<MNode> UpdateTreeAsync(long treeId, MNode treeModel);
        Task DeleteTreeAsync(long treeId);
        Task<MNode?> GetNodeByIdAsync(long nodeId);
        Task<MNode> CreateNodeAsync(long treeId, MNode nodeModel, long? parentId = null);
        Task<MNode> UpdateNodeAsync(long nodeId, MNode nodeModel);
        Task DeleteNodeAsync(long nodeId);
        Task<MNode> GetTreeByNameAsync(string treeName);
        Task<MNode> CreateNodeByNameAsync(string treeName, MNode nodeModel, long parentId);
        Task<MNode> UpdateNodeByNameAsync(string treeName, long nodeId, MNode nodeModel);
        Task DeleteNodeByNameAsync(string treeName, long nodeId);
    }
    
    public class TreeService(IUnitOfWork unitOfWork, IMapper mapper) : ITreeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        
        /// <summary>
        /// Retrieves all trees with their associated nodes from the database
        /// </summary>
        /// <returns>Collection of tree models mapped from entities</returns>
        public async Task<IEnumerable<MNode>> GetAllTreesAsync()
        {
            var trees = await _unitOfWork.Trees.GetAllTreesWithNodesAsync();
            return trees.Select(tree => _mapper.Map<MNode>(tree));
        }
        
        /// <summary>
        /// Retrieves a specific tree by its ID with all associated nodes
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree</param>
        /// <returns>Tree model if found, null otherwise</returns>
        public async Task<MNode?> GetTreeByIdAsync(long treeId)
        {
            var tree = await _unitOfWork.Trees.GetTreeWithNodesAsync(treeId);
            return tree != null ? _mapper.Map<MNode>(tree) : null;
        }
        
        /// <summary>
        /// Creates a new tree in the database
        /// </summary>
        /// <param name="treeModel">The tree model containing the tree data</param>
        /// <returns>The created tree model with generated ID</returns>
        public async Task<MNode> CreateTreeAsync(MNode treeModel)
        {
            var tree = _mapper.Map<Tree>(treeModel);
            await _unitOfWork.Trees.AddAsync(tree);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MNode>(tree);
        }
        
        /// <summary>
        /// Updates an existing tree's information
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree to update</param>
        /// <param name="treeModel">The updated tree data</param>
        /// <returns>The updated tree model</returns>
        /// <exception cref="ArgumentException">Thrown when tree with specified ID is not found</exception>
        public async Task<MNode> UpdateTreeAsync(long treeId, MNode treeModel)
        {
            var existingTree = await _unitOfWork.Trees.GetByIdAsync(treeId);
            if (existingTree == null)
                throw new ArgumentException($"Tree with id {treeId} not found");
                
            existingTree.Name = treeModel.Name;
            existingTree.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.Trees.UpdateAsync(existingTree);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MNode>(existingTree);
        }
        
        /// <summary>
        /// Deletes a tree from the database
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree to delete</param>
        /// <exception cref="ArgumentException">Thrown when tree with specified ID is not found</exception>
        public async Task DeleteTreeAsync(long treeId)
        {
            var tree = await _unitOfWork.Trees.GetByIdAsync(treeId);
            if (tree == null)
                throw new ArgumentException($"Tree with id {treeId} not found");
                
            await _unitOfWork.Trees.DeleteAsync(tree);
            await _unitOfWork.SaveChangesAsync();
        }
        
        /// <summary>
        /// Retrieves a specific node by its ID with all child nodes
        /// </summary>
        /// <param name="nodeId">The unique identifier of the node</param>
        /// <returns>Node model if found, null otherwise</returns>
        public async Task<MNode?> GetNodeByIdAsync(long nodeId)
        {
            var node = await _unitOfWork.Nodes.GetNodeWithChildrenAsync(nodeId);
            return node != null ? _mapper.Map<MNode>(node) : null;
        }
        
        /// <summary>
        /// Creates a new node in a specified tree with optional parent node
        /// </summary>
        /// <param name="treeId">The unique identifier of the tree</param>
        /// <param name="nodeModel">The node model containing the node data</param>
        /// <param name="parentId">Optional parent node ID. If null, node becomes a root node</param>
        /// <returns>The created node model with generated ID</returns>
        /// <exception cref="ArgumentException">Thrown when tree or parent node is not found</exception>
        public async Task<MNode> CreateNodeAsync(long treeId, MNode nodeModel, long? parentId = null)
        {
            var tree = await _unitOfWork.Trees.GetByIdAsync(treeId);
            if (tree == null)
                throw new ArgumentException($"Tree with id {treeId} not found");
                
            if (parentId.HasValue)
            {
                var parent = await _unitOfWork.Nodes.GetByIdAsync(parentId.Value);
                if (parent == null || parent.TreeId != treeId)
                    throw new ArgumentException($"Parent node with id {parentId} not found in tree {treeId}");
            }
            
            var node = _mapper.Map<Node>(nodeModel);
            node.TreeId = treeId;
            node.ParentId = parentId;
            
            await _unitOfWork.Nodes.AddAsync(node);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MNode>(node);
        }
        
        /// <summary>
        /// Updates an existing node's information
        /// </summary>
        /// <param name="nodeId">The unique identifier of the node to update</param>
        /// <param name="nodeModel">The updated node data</param>
        /// <returns>The updated node model</returns>
        /// <exception cref="ArgumentException">Thrown when node with specified ID is not found</exception>
        public async Task<MNode> UpdateNodeAsync(long nodeId, MNode nodeModel)
        {
            var existingNode = await _unitOfWork.Nodes.GetByIdAsync(nodeId);
            if (existingNode == null)
                throw new ArgumentException($"Node with id {nodeId} not found");
                
            existingNode.Name = nodeModel.Name;
            existingNode.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.Nodes.UpdateAsync(existingNode);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MNode>(existingNode);
        }
        
        /// <summary>
        /// Deletes a node from the database. Node can only be deleted if it has no children
        /// </summary>
        /// <param name="nodeId">The unique identifier of the node to delete</param>
        /// <exception cref="ArgumentException">Thrown when node with specified ID is not found</exception>
        /// <exception cref="SecureException">Thrown when node has children and cannot be deleted</exception>
        public async Task DeleteNodeAsync(long nodeId)
        {
            var node = await _unitOfWork.Nodes.GetByIdAsync(nodeId);
            if (node == null)
                throw new ArgumentException($"Node with id {nodeId} not found");
                
            var children = await _unitOfWork.Nodes.GetNodesByTreeIdAsync(node.TreeId);
            if (children.Any(n => n.ParentId == nodeId))
                throw new SecureException("You have to delete all children nodes first");
                
            await _unitOfWork.Nodes.DeleteAsync(node);
            await _unitOfWork.SaveChangesAsync();
        }
        
        /// <summary>
        /// Retrieves or creates a tree by its name. If tree doesn't exist, it will be created automatically
        /// </summary>
        /// <param name="treeName">The name of the tree</param>
        /// <returns>The tree model with all associated nodes</returns>
        public async Task<MNode> GetTreeByNameAsync(string treeName)
        {
            var tree = await _unitOfWork.Trees.GetTreeByNameWithNodesAsync(treeName);
            if (tree == null)
            {
                var newTree = new Tree { Name = treeName };
                await _unitOfWork.Trees.AddAsync(newTree);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<MNode>(newTree);
            }
            
            return _mapper.Map<MNode>(tree);
        }
        
        /// <summary>
        /// Creates a new node in a tree specified by name with a parent node
        /// </summary>
        /// <param name="treeName">The name of the tree</param>
        /// <param name="nodeModel">The node model containing the node data</param>
        /// <param name="parentId">The parent node ID</param>
        /// <returns>The created node model with generated ID</returns>
        /// <exception cref="ArgumentException">Thrown when parent node is not found in the specified tree</exception>
        public async Task<MNode> CreateNodeByNameAsync(string treeName, MNode nodeModel, long parentId)
        {
            var tree = await _unitOfWork.Trees.GetTreeByNameAsync(treeName);
            if (tree == null)
            {
                tree = new Tree { Name = treeName };
                await _unitOfWork.Trees.AddAsync(tree);
                await _unitOfWork.SaveChangesAsync();
            }
            
            var parent = await _unitOfWork.Nodes.GetByIdAsync(parentId);
            if (parent == null || parent.TreeId != tree.Id)
                throw new ArgumentException($"Parent node with id {parentId} not found in tree {treeName}");
            
            var node = _mapper.Map<Node>(nodeModel);
            node.TreeId = tree.Id;
            node.ParentId = parentId;
            
            await _unitOfWork.Nodes.AddAsync(node);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MNode>(node);
        }
        
        /// <summary>
        /// Updates an existing node in a tree specified by name
        /// </summary>
        /// <param name="treeName">The name of the tree</param>
        /// <param name="nodeId">The unique identifier of the node to update</param>
        /// <param name="nodeModel">The updated node data</param>
        /// <returns>The updated node model</returns>
        /// <exception cref="ArgumentException">Thrown when tree or node is not found</exception>
        public async Task<MNode> UpdateNodeByNameAsync(string treeName, long nodeId, MNode nodeModel)
        {
            var tree = await _unitOfWork.Trees.GetTreeByNameAsync(treeName);
            if (tree == null)
                throw new ArgumentException($"Tree with name {treeName} not found");
            
            var existingNode = await _unitOfWork.Nodes.GetByIdAsync(nodeId);
            if (existingNode == null || existingNode.TreeId != tree.Id)
                throw new ArgumentException($"Node with id {nodeId} not found in tree {treeName}");
            
            existingNode.Name = nodeModel.Name;
            existingNode.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.Nodes.UpdateAsync(existingNode);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MNode>(existingNode);
        }
        
        /// <summary>
        /// Deletes a node from a tree specified by name. Node can only be deleted if it has no children
        /// </summary>
        /// <param name="treeName">The name of the tree</param>
        /// <param name="nodeId">The unique identifier of the node to delete</param>
        /// <exception cref="ArgumentException">Thrown when tree or node is not found</exception>
        /// <exception cref="SecureException">Thrown when node has children and cannot be deleted</exception>
        public async Task DeleteNodeByNameAsync(string treeName, long nodeId)
        {
            var tree = await _unitOfWork.Trees.GetTreeByNameAsync(treeName);
            if (tree == null)
                throw new ArgumentException($"Tree with name {treeName} not found");
            
            var node = await _unitOfWork.Nodes.GetByIdAsync(nodeId);
            if (node == null || node.TreeId != tree.Id)
                throw new ArgumentException($"Node with id {nodeId} not found in tree {treeName}");
            
            var children = await _unitOfWork.Nodes.GetNodesByTreeIdAsync(tree.Id);
            if (children.Any(n => n.ParentId == nodeId))
                throw new SecureException("You have to delete all children nodes first");
            
            await _unitOfWork.Nodes.DeleteAsync(node);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
