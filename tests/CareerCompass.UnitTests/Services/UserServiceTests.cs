using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Implementations.Services;
using CareerCompassAPI.Application.DTOs.Auth_DTOs;
using CareerCompassAPI.Persistence.Exceptions;
using CareerCompassAPI.Domain.Identity;

namespace CareerCompassAPI.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetDetailsAsync_UserExists_ReturnsUserDetailsGetDto()
        {
            // Arrange
            var data = new List<AppUser>
{
    new AppUser { Id = "9d315884-138d-4b84-a14b-a2cf48632f1e" },
    // Add other users
}.AsQueryable();

            var mockSet = new Mock<DbSet<AppUser>>();
            var queryProvider = new TestAsyncQueryProvider<AppUser>(data.Provider);

            mockSet.As<IAsyncEnumerable<AppUser>>()
                .Setup(d => d.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<AppUser>(data.GetEnumerator()));

            mockSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(queryProvider);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<CareerCompassDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);


            var userService = new UserService(mockContext.Object);

            // Act
            var sampleGuidString = "9d315884-138d-4b84-a14b-a2cf48632f1e";
            var result = await userService.GetDetailsAsync(sampleGuidString);

            // Assert
            Assert.IsType<UserDetailsGetDto>(result);
        }


        [Fact]
        public async Task GetDetailsAsync_UserNotFound_ThrowsNotFoundException()
        {
            var mockSet = new Mock<DbSet<AppUser>>();
            var mockContext = new Mock<CareerCompassDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var userService = new UserService(mockContext.Object);
            await Assert.ThrowsAsync<NotFoundException>(() => userService.GetDetailsAsync("nonExistentUserId"));
        }
    }
}
