using Microsoft.EntityFrameworkCore;
using TreeApi.Data.Entities;

namespace TreeApi.Data.Repositories
{
    public class ExceptionJournalRepository(TreeApiDbContext context) : Repository<ExceptionJournal>(context), IExceptionJournalRepository
    {
        
        /// <summary>
        /// Retrieves exception journal entries within a specified date range
        /// </summary>
        /// <param name="startDate">Start date for the range (inclusive)</param>
        /// <param name="endDate">End date for the range (inclusive)</param>
        /// <returns>Collection of exception journal entries ordered by timestamp descending</returns>
        public async Task<IEnumerable<ExceptionJournal>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(ej => ej.Timestamp >= startDate && ej.Timestamp <= endDate)
                .OrderByDescending(ej => ej.Timestamp)
                .ToListAsync();
        }
        
        /// <summary>
        /// Retrieves exception journal entries by their event ID
        /// </summary>
        /// <param name="eventId">The unique event identifier</param>
        /// <returns>Collection of exception journal entries ordered by timestamp descending</returns>
        public async Task<IEnumerable<ExceptionJournal>> GetByEventIdAsync(long eventId)
        {
            return await _dbSet
                .Where(ej => ej.EventId == eventId)
                .OrderByDescending(ej => ej.Timestamp)
                .ToListAsync();
        }
    }
}
