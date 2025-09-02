using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TreeApi.Controllers;
using TreeApi.Services;
using TreeApi.Data;
using TreeApi.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using TreeApi.Models;

namespace TreeApi.Tests.Controllers
{
    [TestFixture]
    public class JournalControllerTests
    {
        private JournalController _controller;
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
                options.UseInMemoryDatabase($"JournalTestDatabase_{Guid.NewGuid()}"));
            
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.ExceptionJournalProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _exceptionJournalService = new ExceptionJournalService(_unitOfWork, _mapper);
            
            _journalService = new JournalService(_exceptionJournalService, _mapper);
            _controller = new JournalController(_journalService);
        }

        [Test]
        public void JournalController_Constructor_ShouldNotThrow()
        {
            var controller = new JournalController(_journalService);
            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public async Task JournalController_GetRange_ShouldReturnOkResult()
        {
            var skip = 0;
            var take = 10;
            var filter = new VJournalFilter();
            
            var result = await _controller.GetRange(skip, take, filter);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        public async Task JournalController_GetSingle_ShouldReturnOkResult()
        {
            var journalId = 1L;
            var result = await _controller.GetSingle(journalId);
            
            Assert.That(result, Is.Not.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
