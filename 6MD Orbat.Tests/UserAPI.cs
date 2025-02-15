using _6MD_Orbat.ApiService.Controllers;
using DataStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;
using Moq.EntityFrameworkCore;
namespace _6MD_Orbat.ApiService.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<DB> _mockContext;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mockContext = new Mock<DB>();
            _controller = new UsersController(_mockContext.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0} };
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
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsType<List<User>>(okResult.Value);
            Assert.Single(returnUsers);
        }
        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var users = new List<User> { new User { ID = 1, Name = "Test User", RankID = 1, UserPremissions = 0} };
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
            var result = await _controller.GetUserById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(users[0].ID, returnUser.ID);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
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
            var result = await _controller.GetUserById(20);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreatedAtActionResult_WithUser()
        {
            // Arrange
            var user = new User { ID = 1, Name = "Test User" };
            var mockDbSet = CreateMockDbSet(new List<User>());
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSet.Object);

            // Act
            var result = await _controller.CreateUser(user);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnUser = Assert.IsType<User>(createdAtActionResult.Value);
            
            Assert.Equal(user.ID, returnUser.ID);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent_WhenUserIsUpdated()
        {
            // Arrange
            var user = new User { ID = 1, Name = "Test User" };
            var mockDbSet = CreateMockDbSet(new List<User> { user });
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSet.Object);

            // Act
            var result = await _controller.UpdateUser(1, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<User>());
            _mockContext.Setup(c => c.Users).ReturnsDbSet(mockDbSet.Object);

            // Act
            var result = await _controller.UpdateUser(1, new User());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
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
