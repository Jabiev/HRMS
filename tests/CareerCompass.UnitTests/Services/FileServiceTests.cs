using CareerCompassAPI.Application.Abstraction.Repositories.IFileRepositories;
using CareerCompassAPI.Application.DTOs.File_DTOs;
using CareerCompassAPI.Domain.Identity;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using CareerCompassAPI.Persistence.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace CareerCompass.UnitTests.Services
{
    public class FileServiceTests
    {
        private readonly Mock<ICareerCompassDbContext> _contextMock = new Mock<ICareerCompassDbContext>();
        private readonly Mock<IFileWriteRepository> _fileWriteRepositoryMock = new Mock<IFileWriteRepository>();

        [Fact]
        public async Task CreateAsync_CreatesFileSuccessfully()
        {
            // Arrange
            var fileCreateDto = new FileCreateDto("filename", "blobPath", "containerName", "contentType", 12345, "userId");
            var user = new AppUser { Id = "userId" };

            var userData = new List<AppUser> { user }.AsQueryable();

            var mockSet = new Mock<DbSet<AppUser>>();
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockSet.As<IQueryable<AppUser>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            _contextMock.Setup(c => c.Users).Returns(mockSet.Object);


            _contextMock.Setup(c => c.Users).Returns(mockSet.Object);


            _fileWriteRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<CareerCompassAPI.Domain.Entities.File>()))
                                    .Returns(Task.CompletedTask);

            _fileWriteRepositoryMock.Setup(repo => repo.SaveChangesAsync())
                                    .Returns(Task.CompletedTask);

            var service = new FileService(_contextMock.Object, _fileWriteRepositoryMock.Object);

            // Act
            await service.CreateAsync(fileCreateDto);

            // Assert
            _fileWriteRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<CareerCompassAPI.Domain.Entities.File>()), Times.Once);
            _fileWriteRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public async Task CreateAsync_ThrowsNotFoundException_WhenUserDoesNotExist()
        {

            var fileCreateDto = new FileCreateDto("filename", "blobPath", "containerName", "contentType", 12345, "userId");
            _contextMock.Setup(c => c.Users.FirstOrDefaultAsync(It.IsAny<Expression<Func<AppUser, bool>>>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((AppUser)null);

            var service = new FileService(_contextMock.Object, _fileWriteRepositoryMock.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(fileCreateDto));
        }

    }
}
