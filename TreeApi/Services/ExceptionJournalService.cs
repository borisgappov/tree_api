using AutoMapper;
using TreeApi.Data.Entities;
using TreeApi.Data.UnitOfWork;
using TreeApi.Models;
using TreeApi.Utils;

namespace TreeApi.Services
{
    public interface IExceptionJournalService
    {
        Task<IEnumerable<ExceptionJournal>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<MJournal>> GetByEventIdAsync(long eventId);
        Task<MJournal?> GetByIdAsync(long id);
        Task<MJournal> LogExceptionAsync(Exception exception, string? queryParameters = null, string? bodyParameters = null, string? requestPath = null, string? httpMethod = null);
    }
    
    public class ExceptionJournalService(IUnitOfWork unitOfWork, IMapper mapper) : IExceptionJournalService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        
        /// <summary>
        /// Retrieves exception journal entries within a specified date range
        /// </summary>
        /// <param name="startDate">Start date for the range (inclusive)</param>
        /// <param name="endDate">End date for the range (inclusive)</param>
        /// <returns>Collection of exception journal entries</returns>
        public async Task<IEnumerable<ExceptionJournal>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var journals = await _unitOfWork.ExceptionJournals.GetByDateRangeAsync(startDate, endDate);
            return journals;
        }
        
        /// <summary>
        /// Retrieves exception journal entries by their event ID
        /// </summary>
        /// <param name="eventId">The unique event identifier</param>
        /// <returns>Collection of journal models mapped from entities</returns>
        public async Task<IEnumerable<MJournal>> GetByEventIdAsync(long eventId)
        {
            var journals = await _unitOfWork.ExceptionJournals.GetByEventIdAsync(eventId);
            return journals.Select(journal => _mapper.Map<MJournal>(journal));
        }
        
        /// <summary>
        /// Retrieves a specific exception journal entry by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the journal entry</param>
        /// <returns>Journal model if found, null otherwise</returns>
        public async Task<MJournal?> GetByIdAsync(long id)
        {
            var journal = await _unitOfWork.ExceptionJournals.GetByIdAsync(id);
            return journal != null ? _mapper.Map<MJournal>(journal) : null;
        }
        
        /// <summary>
        /// Logs an exception to the journal with request context information
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="queryParameters">Optional query parameters from the request</param>
        /// <param name="bodyParameters">Optional body parameters from the request</param>
        /// <param name="requestPath">Optional request path</param>
        /// <param name="httpMethod">Optional HTTP method</param>
        /// <returns>The created journal entry model with generated event ID</returns>
        public async Task<MJournal> LogExceptionAsync(Exception exception, string? queryParameters = null, string? bodyParameters = null, string? requestPath = null, string? httpMethod = null)
        {
            var journal = new ExceptionJournal
            {
                EventId = SnowflakeIdGenerator.NextId(),
                Timestamp = DateTime.UtcNow,
                QueryParameters = queryParameters,
                BodyParameters = bodyParameters,
                StackTrace = exception.StackTrace ?? string.Empty,
                ExceptionMessage = exception.Message,
                RequestPath = requestPath,
                HttpMethod = httpMethod
            };
            
            await _unitOfWork.ExceptionJournals.AddAsync(journal);
            await _unitOfWork.SaveChangesAsync();
            
            return _mapper.Map<MJournal>(journal);
        }
    }
}
