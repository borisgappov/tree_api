using AutoMapper;
using TreeApi.Models;

namespace TreeApi.Services
{
    public interface IJournalService
    {
        Task<MRangeMJournalInfo> GetRangeAsync(int skip, int take, VJournalFilter? filter);
        Task<MJournal?> GetSingleAsync(long id);
    }
    
    public class JournalService(IExceptionJournalService exceptionJournalService, IMapper mapper) : IJournalService
    {
        
        /// <summary>
        /// Retrieves a paginated range of journal entries with optional filtering
        /// </summary>
        /// <param name="skip">Number of items to skip for pagination</param>
        /// <param name="take">Maximum number of items to return</param>
        /// <param name="filter">Optional filter criteria for date range and search</param>
        /// <returns>Paginated collection of journal entries with total count</returns>
        public async Task<MRangeMJournalInfo> GetRangeAsync(int skip, int take, VJournalFilter? filter)
        {
            var startDate = filter?.From ?? DateTime.UtcNow.AddDays(-30);
            var endDate = filter?.To ?? DateTime.UtcNow;
            
            var journals = await exceptionJournalService.GetByDateRangeAsync(startDate, endDate);
            
            var query = journals.AsQueryable();

            if (filter != null && !string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(j => (j.ExceptionMessage != null && j.ExceptionMessage.Contains(filter.Search, StringComparison.OrdinalIgnoreCase)) ||
                                        (j.StackTrace != null && j.StackTrace.Contains(filter.Search, StringComparison.OrdinalIgnoreCase)));
            }

            var totalCount = query.Count();
            var items = query
                .Skip(skip)
                .Take(take)
                .Select(j => mapper.Map<MJournalInfo>(j))
                .ToList();

            return new MRangeMJournalInfo
            {
                Skip = skip,
                Count = totalCount,
                Items = items
            };
        }
        
        /// <summary>
        /// Retrieves a single journal entry by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the journal entry</param>
        /// <returns>Journal entry if found, null otherwise</returns>
        public async Task<MJournal?> GetSingleAsync(long id)
        {
            return await exceptionJournalService.GetByIdAsync(id);
        }
    }
}
