using NUnit.Framework;
using TreeApi.Data;
using TreeApi.Data.Repositories;
using TreeApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TreeApi.Tests.Data.Repositories
{
    [TestFixture]
    public class TreeRepositoryTests
    {
        private TreeRepository _repository;
        private TreeApiDbContext _context;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"TreeRepositoryTestDatabase_{Guid.NewGuid()}"));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _repository = new TreeRepository(_context);
        }

        [Test]
        public async Task TreeRepository_GetAllAsync_ShouldReturnEmptyCollection()
        {
            var result = await _repository.GetAllAsync();
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task TreeRepository_GetByIdAsync_ShouldReturnNullForNonExistentId()
        {
            var result = await _repository.GetByIdAsync(999L);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TreeRepository_AddAsync_ShouldAddNewTree()
        {
            var tree = new Tree { Name = "Test Tree" };
            
            await _repository.AddAsync(tree);
            await _context.SaveChangesAsync();
            
            var savedTree = await _repository.GetByIdAsync(tree.Id);
            Assert.That(savedTree, Is.Not.Null);
            Assert.That(savedTree.Name, Is.EqualTo("Test Tree"));
        }

        [Test]
        public async Task TreeRepository_UpdateAsync_ShouldUpdateExistingTree()
        {
            var tree = new Tree { Name = "Original Name" };
            await _repository.AddAsync(tree);
            await _context.SaveChangesAsync();
            
            tree.Name = "Updated Name";
            await _repository.UpdateAsync(tree);
            await _context.SaveChangesAsync();
            
            var updatedTree = await _repository.GetByIdAsync(tree.Id);
            Assert.That(updatedTree.Name, Is.EqualTo("Updated Name"));
        }

        [Test]
        public async Task TreeRepository_DeleteAsync_ShouldRemoveTree()
        {
            var tree = new Tree { Name = "To Delete" };
            await _repository.AddAsync(tree);
            await _context.SaveChangesAsync();
            
            await _repository.DeleteAsync(tree);
            await _context.SaveChangesAsync();
            
            var deletedTree = await _repository.GetByIdAsync(tree.Id);
            Assert.That(deletedTree, Is.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }
    }
}
