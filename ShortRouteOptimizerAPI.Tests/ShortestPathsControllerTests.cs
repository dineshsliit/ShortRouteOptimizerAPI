using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShortRouteOptimizerAPI.Controllers;
using ShortRouteOptimizerAPI.Models;
using ShortRouteOptimizerAPI.Services;
using NUnit.Framework;
using Microsoft.Extensions.Caching.Memory;
using ShortRouteOptimizerAPI.DataAccess;
using Microsoft.Extensions.Options;

namespace ShortRouteOptimizerAPI.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="ShortestPathsController"/> class.
    /// </summary>
    [TestFixture]
    public class ShortestPathsControllerTests
    {
        private Mock<IShortestPathService> _shortestPathServiceMock;
        private Mock<IMemoryCache> _memoryCacheMock;
        private Mock<IOptions<CacheSettings>> _cacheSettingsMock;
        private ShortestPathsController _shortestPathsController;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _shortestPathServiceMock = new Mock<IShortestPathService>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _cacheSettingsMock = new Mock<IOptions<CacheSettings>>();

            var cacheSettings = new CacheSettings 
            { 
                ShortestPathCacheKey = "ShortestPath_{0}_{1}", 
                CacheDurationMinutes = 5 
            }; 
            
            _cacheSettingsMock.Setup(x => x.Value).Returns(cacheSettings);


            _shortestPathsController = new ShortestPathsController(_shortestPathServiceMock.Object,_cacheSettingsMock.Object,_memoryCacheMock.Object);
        }

        /// <summary>
        /// Tests that the <see cref="ShortestPathsController.GetShortestPath"/> method returns an Ok result for nearest node navigation.
        /// </summary>
        [Test]
        public void GetShortestPath_ShouldReturnOkResult_NearestNodeNavigation()
        {
            // Arrange
            var fromNodeName = "A";
            var toNodeName = "B";

            var shortestPathData = new ShortestPathData
            {
                NodeNames = new List<string> { "A", "B" },
                Distance = 4
            };

            // Mocking Cache
            var cacheKey = string.Format("ShortestPath_{0}_{1}", fromNodeName, toNodeName);
            object cacheEntry = null;
            
            _memoryCacheMock .Setup(x => x.TryGetValue(cacheKey, out cacheEntry)) .Returns(false);            
            _memoryCacheMock .Setup(x => x.CreateEntry(It.IsAny<object>())) .Returns(Mock.Of<ICacheEntry>()); 

            _shortestPathServiceMock
                .Setup(x => x.GetShortestPathFromNodes(fromNodeName, toNodeName))
                .Returns(shortestPathData);

            // Act
            var result = _shortestPathsController.GetShortestPath(fromNodeName, toNodeName);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returnValue = okResult.Value as ShortestPathData;
            returnValue.NodeNames.Should().Equal(new List<string> { "A", "B" });
            returnValue.Distance.Should().Be(4);
        }

        /// <summary>
        /// Tests that the <see cref="ShortestPathsController.GetShortestPath"/> method returns an Ok result for edge nodes navigation.
        /// </summary>
        [Test]
        public void GetShortestPath_ShouldReturnOkResult_NodesEdgesNavigation()
        {
            // Arrange
            var fromNodeName = "A";
            var toNodeName = "I";

            var shortestPathData = new ShortestPathData
            {
                NodeNames = new List<string> { "A", "B", "F", "G", "I" },
                Distance = 15
            };

            // Mocking Cache
            var cacheKey = string.Format("ShortestPath_{0}_{1}", fromNodeName, toNodeName);
            object cacheEntry = null;

            _memoryCacheMock.Setup(x => x.TryGetValue(cacheKey, out cacheEntry)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            _shortestPathServiceMock
                .Setup(x => x.GetShortestPathFromNodes(fromNodeName, toNodeName))
                .Returns(shortestPathData);

            // Act
            var result = _shortestPathsController.GetShortestPath(fromNodeName, toNodeName);

            // Assert
            var okResult = result.Result as OkObjectResult;

            okResult.Should().NotBeNull();

            var returnValue = okResult.Value as ShortestPathData;

            returnValue.NodeNames.Should().Equal(new List<string> { "A", "B", "F", "G", "I" });
            returnValue.Distance.Should().Be(15);
        }

        /// <summary>
        /// Tests that the <see cref="ShortestPathsController.GetShortestPath"/> method returns a BadRequest result on exception.
        /// </summary>
        [Test]
        public void GetShortestPath_ShouldReturnBadRequest_OnException()
        {
            // Arrange
            var fromNodeName = "A";
            var toNodeName = "B";

            // Mocking Cache
            var cacheKey = string.Format("ShortestPath_{0}_{1}", fromNodeName, toNodeName);
            object cacheEntry = null;

            _memoryCacheMock.Setup(x => x.TryGetValue(cacheKey, out cacheEntry)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            _shortestPathServiceMock.Setup(service => service
                                    .GetShortestPathFromNodes(fromNodeName, toNodeName))
                                    .Throws(new Exception("Test exception"));

            // Act
            var result = _shortestPathsController.GetShortestPath(fromNodeName, toNodeName);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            var exception = badRequestResult.Value as Exception;
            exception.Message.Should().Be("Test exception");
        }

        /// <summary>
        /// Tests that the <see cref="ShortestPathsController.GetShortestPath"/> method returns a BadRequest result for null parameters.
        /// </summary>
        [Test]
        public void GetShortestPath_ShouldReturnBadRequest_ForNullParameters()
        {
            // Arrange
            string fromNodeName = null;
            string toNodeName = null;

            // Mocking Cache
            var cacheKey = string.Format("ShortestPath_{0}_{1}", fromNodeName, toNodeName);
            object cacheEntry = null;

            _memoryCacheMock.Setup(x => x.TryGetValue(cacheKey, out cacheEntry)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());


            // Act
            var result = _shortestPathsController.GetShortestPath(fromNodeName, toNodeName);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
        }

        /// <summary>
        /// Tests that the <see cref="ShortestPathsController.GetShortestPath"/> method returns a BadRequest result for non-existent nodes.
        /// </summary>
        [Test]
        public void GetShortestPath_ShouldReturnBadRequest_ForNonExistentNodes()
        {
            // Arrange
            var fromNodeName = "X";
            var toNodeName = "Y";

            // Mocking Cache
            var cacheKey = string.Format("ShortestPath_{0}_{1}", fromNodeName, toNodeName);
            object cacheEntry = null;

            _memoryCacheMock.Setup(x => x.TryGetValue(cacheKey, out cacheEntry)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());


            _shortestPathServiceMock.Setup(x => x.GetShortestPathFromNodes(fromNodeName, toNodeName)).Throws(new Exception("Nodes not found"));

            // Act
            var result = _shortestPathsController.GetShortestPath(fromNodeName, toNodeName);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            var exception = badRequestResult.Value as Exception;
            exception.Message.Should().Be("Nodes not found");
        }

        /// <summary>
        /// Tests that the <see cref="ShortestPathsController.GetShortestPath"/> method returns an Ok result for self-looping nodes.
        /// </summary>
        [Test]
        public void GetShortestPath_ShouldReturnOkResult_ForSelfLoopingNode()
        {
            // Arrange
            var fromNodeName = "A";
            var toNodeName = "A";

            var shortestPathData = new ShortestPathData { NodeNames = new List<string> { "A" }, Distance = 0 };

            // Mocking Cache
            var cacheKey = string.Format("ShortestPath_{0}_{1}", fromNodeName, toNodeName);
            object cacheEntry = null;

            _memoryCacheMock.Setup(x => x.TryGetValue(cacheKey, out cacheEntry)).Returns(false);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            _shortestPathServiceMock.Setup(x => x.GetShortestPathFromNodes(fromNodeName, toNodeName)).Returns(shortestPathData);

            // Act
            var result = _shortestPathsController.GetShortestPath(fromNodeName, toNodeName);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            var returnValue = okResult.Value as ShortestPathData;
            returnValue.NodeNames.Should().Equal(new List<string> { "A" });
            returnValue.Distance.Should().Be(0);
        }
    }
}



