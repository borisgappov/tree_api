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
using TreeApi.Data.Entities;

namespace TreeApi.Tests.Controllers
{
    [TestFixture]
    public class TreeNodeControllerTests
    {
        private TreeNodeController _controller;
        private ITreeService _treeService;
        private TreeApiDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<TreeApiDbContext>(options =>
                options.UseInMemoryDatabase($"TreeNodeTestDatabase_{Guid.NewGuid()}"));
            
            services.AddAutoMapper(typeof(TreeApi.Services.Mappers.NodeProfile));
            
            var serviceProvider = services.BuildServiceProvider();
            _context = serviceProvider.GetRequiredService<TreeApiDbContext>();
            _unitOfWork = new UnitOfWork(_context);
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            
            _treeService = new TreeService(_unitOfWork, _mapper);
            _controller = new TreeNodeController(_treeService);
        }

        [Test]
        public void TreeNodeController_Constructor_ShouldNotThrow()
        {
            var controller = new TreeNodeController(_treeService);
            Assert.That(controller, Is.Not.Null);
        }

        [Test]
        public async Task TreeNodeController_Create_ShouldReturnOkResult()
        {
            // Arrange - Create test tree first
            var treeName = "test-tree";
            var tree = await _treeService.GetTreeByNameAsync(treeName);
            
            // Create a root node first
            var rootNodeModel = new MNode { Name = "Root Node" };
            var rootNode = await _treeService.CreateNodeAsync(tree.Id, rootNodeModel, null);
            
            var parentNodeId = rootNode.Id; // Use the created root node as parent
            var nodeName = "Test Node";
            
            // Act
            var result = await _controller.Create(treeName, parentNodeId, nodeName);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task TreeNodeController_Delete_ShouldReturnOkResult()
        {
            // Arrange - Create test tree and node first
            var treeName = "test-tree";
            var tree = await _treeService.GetTreeByNameAsync(treeName);
            
            // Create a root node first
            var rootNodeModel = new MNode { Name = "Root Node" };
            var rootNode = await _treeService.CreateNodeAsync(tree.Id, rootNodeModel, null);
            
            // Create a child node to delete
            var childNodeModel = new MNode { Name = "Node To Delete" };
            var childNode = await _treeService.CreateNodeAsync(tree.Id, childNodeModel, rootNode.Id);
            
            var nodeId = childNode.Id; // Use the created child node for deletion
            
            // Act
            var result = await _controller.Delete(treeName, nodeId);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task TreeNodeController_Rename_ShouldReturnOkResult()
        {
            // Arrange - Create test tree and node first
            var treeName = "test-tree";
            var tree = await _treeService.GetTreeByNameAsync(treeName);
            
            // Create a root node first
            var rootNodeModel = new MNode { Name = "Root Node" };
            var rootNode = await _treeService.CreateNodeAsync(tree.Id, rootNodeModel, null);
            
            // Create a child node to rename
            var childNodeModel = new MNode { Name = "Node To Rename" };
            var childNode = await _treeService.CreateNodeAsync(tree.Id, childNodeModel, rootNode.Id);
            
            var nodeId = childNode.Id; // Use the created child node for rename
            var newNodeName = "Updated Node";
            
            // Act
            var result = await _controller.Rename(treeName, nodeId, newNodeName);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _unitOfWork?.Dispose();
            _context?.Dispose();
        }
    }
}
