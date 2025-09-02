using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TreeApi.Controllers;
using TreeApi.Services;
using TreeApi.Data;
using TreeApi.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace TreeApi.Tests.Controllers
{
    [TestFixture]
    public class TreeControllerTests
    {
        private TreeController _controller;
        private ITreeService _treeService;
        private TreeApiDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            // Create in-memory database for tests
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"TreeControllerTestDatabase_{Guid.NewGuid()}"));
            
            // Add AutoMapper
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.TreeProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            
            // Create services
            _treeService = new TreeService(_unitOfWork, _mapper);
            _controller = new TreeController(_treeService);
        }

        [Test]
        public void TreeController_Constructor_ShouldNotThrow()
        {
            // Arrange & Act
            var controller = new TreeController(_treeService);
            
            // Assert
            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public async Task TreeController_Get_ShouldReturnOkResult()
        {
            // Arrange
            var treeName = "test-tree";
            
            // Act
            var result = await _controller.Get(treeName);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
