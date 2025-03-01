using _6MD.ApiService.Controllers;
using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _6MD.ApiService.Tests.Controllers
{
    public class GroupControllerTests
    {
        private readonly Mock<DB> _mockContext;
        private readonly GroupController _controller;

        public GroupControllerTests()
        {
            _mockContext = new Mock<DB>();
            _controller = new GroupController(_mockContext.Object);
        }

        [Fact]
        public async Task GetGroups_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0 } };
            var ranks = new List<Rank> { new Rank { ID = 1, Name = "Test Rank" } };
            var trining = new List<Training> { new Training { ID = 1, Name = "Test Training" } };
            var groups = new List<Groups> { new Groups { ID = 1, Name = "Test Group" } };
            var trainers = new List<Trainers> { new Trainers { ID = 1, Active = true, HeadTrainer = true, TrainingID = 1 } };
            var mockDbSet = CreateMockDbSet(users);
            var mockDbSetRanks = CreateMockDbSet(ranks);
            var mockDbSetTraining = CreateMockDbSet(trining);
            var mockDbSetGroups = CreateMockDbSet(groups);
            var mockDbSetTrainers = CreateMockDbSet(trainers);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Ranks).ReturnsDbSet(mockDbSetRanks.Object);
            _mockContext.Setup(c => c.Trainings).ReturnsDbSet(mockDbSetTraining.Object);
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSetGroups.Object);
            _mockContext.Setup(c => c.Trainers).ReturnsDbSet(mockDbSetTrainers.Object);

            // Act
            var result = await _controller.GetGroups();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returngroups = Assert.IsType<List<Groups>>(okResult.Value);
            Assert.Single(returngroups);
        }

        [Fact]
        public async Task GetGroupById_ReturnsOkResult_WithGroup()
        {
            // Arrange
            var users = new List<User> { new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0 } };
            var ranks = new List<Rank> { new Rank { ID = 1, Name = "Test Rank" } };
            var trining = new List<Training> { new Training { ID = 1, Name = "Test Training" } };
            var groups = new List<Groups> { new Groups { ID = 1, Name = "Test Group" } };
            var trainers = new List<Trainers> { new Trainers { ID = 1, Active = true, HeadTrainer = true, TrainingID = 1 } };
            var mockDbSet = CreateMockDbSet(users);
            var mockDbSetRanks = CreateMockDbSet(ranks);
            var mockDbSetTraining = CreateMockDbSet(trining);
            var mockDbSetGroups = CreateMockDbSet(groups);
            var mockDbSetTrainers = CreateMockDbSet(trainers);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Ranks).ReturnsDbSet(mockDbSetRanks.Object);
            _mockContext.Setup(c => c.Trainings).ReturnsDbSet(mockDbSetTraining.Object);
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSetGroups.Object);
            _mockContext.Setup(c => c.Trainers).ReturnsDbSet(mockDbSetTrainers.Object);

            // Act
            var result = await _controller.GetGroupById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnGroup = Assert.IsType<Groups>(okResult.Value);
            Assert.Equal(groups[0].ID, returnGroup.ID);
        }

        [Fact]
        public async Task GetGroupById_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            var users = new List<User> { new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0 } };
            var ranks = new List<Rank> { new Rank { ID = 1, Name = "Test Rank" } };
            var trining = new List<Training> { new Training { ID = 1, Name = "Test Training" } };
            var groups = new List<Groups> { new Groups { ID = 1, Name = "Test Group" } };
            var trainers = new List<Trainers> { new Trainers { ID = 1, Active = true, HeadTrainer = true, TrainingID = 1 } };
            var mockDbSet = CreateMockDbSet(users);
            var mockDbSetRanks = CreateMockDbSet(ranks);
            var mockDbSetTraining = CreateMockDbSet(trining);
            var mockDbSetGroups = CreateMockDbSet(groups);
            var mockDbSetTrainers = CreateMockDbSet(trainers);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Ranks).ReturnsDbSet(mockDbSetRanks.Object);
            _mockContext.Setup(c => c.Trainings).ReturnsDbSet(mockDbSetTraining.Object);
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSetGroups.Object);
            _mockContext.Setup(c => c.Trainers).ReturnsDbSet(mockDbSetTrainers.Object);

            // Act
            var result = await _controller.GetGroupById(20);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task CreateGroup_ReturnsCreatedAtActionResult_WithGroup()
        {
            // Arrange
            var group = new Groups { ID = 1, Name = "Test Group" };
            var mockDbSet = CreateMockDbSet(new List<Groups>());
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);

            // Act
            var result = await _controller.CreateGroup(group);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnGroup = Assert.IsType<Groups>(createdAtActionResult.Value);

            Assert.Equal(group.ID, returnGroup.ID);
        }

        [Fact]
        public async Task UpdateGroup_ReturnsNoContent_WhenGroupIsUpdated()
        {
            // Arrange
            var user = new Groups { ID = 1, Name = "Test Group" };
            var mockDbSet = CreateMockDbSet(new List<Groups> { user });
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);

            // Act
            var result = await _controller.UpdateGroup(1, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateGroup_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Groups>());
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);

            // Act
            var result = await _controller.UpdateGroup(1, new Groups());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddUserToGroup_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Groups>());
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);
            // Act
            var result = await _controller.AddUserToGroup(1, 1);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddUserToGroup_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var group = new Groups { ID = 1, Name = "Test Group" };
            var mockDbSet = CreateMockDbSet(new List<Groups> { group });
            var mockDbSetUsers = CreateMockDbSet(new List<User>());
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSetUsers.Object);
            // Act
            var result = await _controller.AddUserToGroup(1, 1);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task AddUserToGroup_ReturnsNoContent_WhenUserIsAddedToGroup()
        {
            // Arrange
            var group = new Groups { ID = 1, Name = "Test Group" };
            var user = new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0 };
            var mockDbSet = CreateMockDbSet(new List<Groups> { group });
            var mockDbSetUsers = CreateMockDbSet(new List<User> { user });
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSetUsers.Object);
            // Act
            var result = await _controller.AddUserToGroup(1, 1);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task RemoveUserFromGroup_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Groups>());
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);
            // Act
            var result = await _controller.RemoveUserFromGroup(1, 1);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RemoveUserFromGroup_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var group = new Groups { ID = 1, Name = "Test Group" };
            var mockDbSet = CreateMockDbSet(new List<Groups> { group });
            var mockDbSetUsers = CreateMockDbSet(new List<User>());
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSetUsers.Object);
            // Act
            var result = await _controller.RemoveUserFromGroup(1, 1);
            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task RemoveUserFromGroup_ReturnsNoContent_WhenUserIsRemovedFromGroup()
        {
            // Arrange
            var group = new Groups { ID = 1, Name = "Test Group" };
            var user = new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0 };
            var mockDbSet = CreateMockDbSet(new List<Groups> { group });
            var mockDbSetUsers = CreateMockDbSet(new List<User> { user });
            _mockContext.Setup(c => c.Groups).ReturnsDbSet(mockDbSet.Object);
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSetUsers.Object);
            // Act
            var result = await _controller.RemoveUserFromGroup(1, 1);
            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> elements) where T : class
        {
            var queryable = elements.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockDbSet;
        }
    }
}
