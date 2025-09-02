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
    public class PartnerControllerTests
    {
        private PartnerController _controller;
        private IPartnerService _partnerService;
        private TreeApiDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"PartnerTestDatabase_{Guid.NewGuid()}"));
            
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.PartnerProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            
            _partnerService = new PartnerService(_unitOfWork, _mapper);
            _controller = new PartnerController(_partnerService);
        }

        [Test]
        public void PartnerController_Constructor_ShouldNotThrow()
        {
            var controller = new PartnerController(_partnerService);
            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public async Task PartnerController_RememberMe_ShouldReturnOkResult()
        {
            var code = "TEST001";
            var result = await _controller.RememberMe(code);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
