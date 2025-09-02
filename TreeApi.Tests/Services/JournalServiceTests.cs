using NUnit.Framework;
using TreeApi.Services;
using TreeApi.Data;
using TreeApi.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using TreeApi.Models;

namespace TreeApi.Tests.Services
{
    [TestFixture]
    public class JournalServiceTests
    {
        private IJournalService _journalService;
        private TreeApiDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IExceptionJournalService _exceptionJournalService;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"JournalServiceTestDatabase_{Guid.NewGuid()}"));
            
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.ExceptionJournalProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _exceptionJournalService = new ExceptionJournalService(_unitOfWork, _mapper);
            
            _journalService = new JournalService(_exceptionJournalService, _mapper);
        }

        [Test]
        public async Task JournalService_GetRangeAsync_ShouldReturnEmptyCollection()
        {
            var skip = 0;
            var take = 10;
            var filter = new VJournalFilter();
            
            var result = await _journalService.GetRangeAsync(skip, take, filter);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Items, Is.Empty);
        }

        [Test]
        public async Task JournalService_GetSingleAsync_ShouldReturnNullForNonExistentId()
        {
            var result = await _journalService.GetSingleAsync(999L);
            
            Assert.That(result, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
