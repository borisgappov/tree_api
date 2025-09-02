using Microsoft.EntityFrameworkCore;

namespace TreeApi.Data.Repositories
{
    public class Repository<T>(TreeApiDbContext context) : IRepository<T> where T : class
    {
        protected readonly TreeApiDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();
        
        /// <summary>
        /// Retrieves an entity by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <returns>The entity if found, null otherwise</returns>
        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        /// <summary>
        /// Retrieves all entities of the specified type
        /// </summary>
        /// <returns>Collection of all entities</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        
        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        
        /// <summary>
        /// Updates an existing entity in the database
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }
        
        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }
        
        /// <summary>
        /// Checks if an entity exists by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <returns>True if entity exists, false otherwise</returns>
        public virtual async Task<bool> ExistsAsync(long id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}
