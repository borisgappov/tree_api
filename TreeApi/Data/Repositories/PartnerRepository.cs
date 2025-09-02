using Microsoft.EntityFrameworkCore;
using TreeApi.Data.Entities;

namespace TreeApi.Data.Repositories
{
    public class PartnerRepository(TreeApiDbContext context) : Repository<Partner>(context), IPartnerRepository
    {
        
        /// <summary>
        /// Retrieves a partner by its unique code
        /// </summary>
        /// <param name="code">The unique code of the partner</param>
        /// <returns>The partner if found, null otherwise</returns>
        public async Task<Partner?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Code == code);
        }
        
        /// <summary>
        /// Checks if a partner exists with the specified code
        /// </summary>
        /// <param name="code">The unique code to check</param>
        /// <returns>True if partner exists, false otherwise</returns>
        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _dbSet
                .AnyAsync(p => p.Code == code);
        }
    }
}
