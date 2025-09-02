using NUnit.Framework;
using TreeApi.Data;
using TreeApi.Data.Repositories;
using TreeApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TreeApi.Tests.Data.Repositories
{
    [TestFixture]
    public class NodeRepositoryTests
    {
        private NodeRepository _repository;
        private TreeApiDbContext _context;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"NodeRepositoryTestDatabase_{Guid.NewGuid()}"));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _repository = new NodeRepository(_context);
        }

        [Test]
        public async Task NodeRepository_GetAllAsync_ShouldReturnEmptyCollection()
        {
            var result = await _repository.GetAllAsync();
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task NodeRepository_GetByIdAsync_ShouldReturnNullForNonExistentId()
        {
            var result = await _repository.GetByIdAsync(999L);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task NodeRepository_AddAsync_ShouldAddNewNode()
        {
            var node = new Node { Name = "Test Node", TreeId = 1 };
            
            await _repository.AddAsync(node);
            await _context.SaveChangesAsync();
            
            var savedNode = await _repository.GetByIdAsync(node.Id);
            Assert.That(savedNode, Is.Not.Null);
            Assert.That(savedNode.Name, Is.EqualTo("Test Node"));
        }

        [Test]
        public async Task NodeRepository_GetNodesByTreeIdAsync_ShouldReturnEmptyCollection()
        {
            var result = await _repository.GetNodesByTreeIdAsync(1L);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task NodeRepository_GetRootNodesByTreeIdAsync_ShouldReturnEmptyCollection()
        {
            var result = await _repository.GetRootNodesByTreeIdAsync(1L);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}
