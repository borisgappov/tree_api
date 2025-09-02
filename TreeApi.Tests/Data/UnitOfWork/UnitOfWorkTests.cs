using NUnit.Framework;
using TreeApi.Data;
using TreeApi.Data.UnitOfWork;
using TreeApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TreeApi.Tests.Data.UnitOfWork
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private IUnitOfWork _unitOfWork;
        private TreeApiDbContext _context;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"UnitOfWorkTestDatabase_{Guid.NewGuid()}"));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new TreeApi.Data.UnitOfWork.UnitOfWork(_context);
        }

        [Test]
        public void UnitOfWork_Constructor_ShouldNotThrow()
        {
            var unitOfWork = new TreeApi.Data.UnitOfWork.UnitOfWork(_context);
            Assert.That(unitOfWork, Is.Not.Null);
        }

        [Test]
        public void UnitOfWork_Trees_ShouldReturnTreeRepository()
        {
            var repository = _unitOfWork.Trees;
            
            Assert.That(repository, Is.Not.Null);
            Assert.That(repository, Is.InstanceOf<ITreeRepository>());
        }

        [Test]
        public void UnitOfWork_Nodes_ShouldReturnNodeRepository()
        {
            var repository = _unitOfWork.Nodes;
            
            Assert.That(repository, Is.Not.Null);
            Assert.That(repository, Is.InstanceOf<INodeRepository>());
        }

        [Test]
        public void UnitOfWork_Partners_ShouldReturnPartnerRepository()
        {
            var repository = _unitOfWork.Partners;
            
            Assert.That(repository, Is.Not.Null);
            Assert.That(repository, Is.InstanceOf<IPartnerRepository>());
        }

        [Test]
        public void UnitOfWork_ExceptionJournals_ShouldReturnExceptionJournalRepository()
        {
            var repository = _unitOfWork.ExceptionJournals;
            
            Assert.That(repository, Is.Not.Null);
            Assert.That(repository, Is.InstanceOf<IExceptionJournalRepository>());
        }

        [Test]
        public async Task UnitOfWork_SaveChangesAsync_ShouldCompleteSuccessfully()
        {
            await _unitOfWork.SaveChangesAsync();
            
            // Should not throw any exception
            Assert.Pass();
        }

        [Test]
        public void UnitOfWork_Dispose_ShouldNotThrow()
        {
            _unitOfWork.Dispose();
            
            // Should not throw any exception
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
