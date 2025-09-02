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
    public class PartnerServiceTests
    {
        private IPartnerService _partnerService;
        private TreeApiDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"PartnerServiceTestDatabase_{Guid.NewGuid()}"));
            
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.PartnerProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            
            _partnerService = new PartnerService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task PartnerService_GetAllPartnersAsync_ShouldReturnEmptyCollection()
        {
            var result = await _partnerService.GetAllPartnersAsync();
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task PartnerService_GetPartnerByIdAsync_ShouldReturnNullForNonExistentId()
        {
            var result = await _partnerService.GetPartnerByIdAsync(999L);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task PartnerService_CreatePartnerAsync_ShouldCreateNewPartner()
        {
            var partnerModel = new MPartner { Code = "TEST001" };
            
            var result = await _partnerService.CreatePartnerAsync(partnerModel);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Code, Is.EqualTo("TEST001"));
        }

        [Test]
        public async Task PartnerService_UpdatePartnerAsync_ShouldThrowExceptionForNonExistentId()
        {
            var partnerModel = new MPartner { Id = 999L, Code = "TEST001" };
            
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _partnerService.UpdatePartnerAsync(999L, partnerModel));
        }

        [Test]
        public async Task PartnerService_DeletePartnerAsync_ShouldThrowExceptionForNonExistentId()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _partnerService.DeletePartnerAsync(999L));
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
