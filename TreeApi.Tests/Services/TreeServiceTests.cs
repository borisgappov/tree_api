using NUnit.Framework;
using TreeApi.Services;
using TreeApi.Data;
using TreeApi.Data.UnitOfWork;
using TreeApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using TreeApi.Models;
using TreeApi.Data.Entities;

namespace TreeApi.Tests.Services
{
    [TestFixture]
    public class TreeServiceTests
    {
        private ITreeService _treeService;
        private TreeApiDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"TreeServiceTestDatabase_{Guid.NewGuid()}"));
            
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.TreeProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            
            _treeService = new TreeService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task TreeService_GetAllTreesAsync_ShouldReturnEmptyCollection()
        {
            var result = await _treeService.GetAllTreesAsync();
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task TreeService_GetTreeByIdAsync_ShouldReturnNullForNonExistentId()
        {
            var result = await _treeService.GetTreeByIdAsync(999L);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TreeService_CreateTreeAsync_ShouldCreateNewTree()
        {
            var treeModel = new MNode { Name = "Test Tree" };
            
            var result = await _treeService.CreateTreeAsync(treeModel);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Tree"));
        }

        [Test]
        public async Task TreeService_GetTreeByNameAsync_ShouldCreateTreeIfNotExists()
        {
            var treeName = "New Test Tree";
            
            var result = await _treeService.GetTreeByNameAsync(treeName);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(treeName));
        }

        [Test]
        public async Task TreeService_GetNodeByIdAsync_ShouldReturnNullForNonExistentNode()
        {
            var result = await _treeService.GetNodeByIdAsync(999L);
            
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
