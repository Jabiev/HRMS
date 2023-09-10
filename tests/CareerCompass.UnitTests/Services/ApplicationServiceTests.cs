using CareerCompassAPI.Application.Abstraction.Repositories.IJobApplicationRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IJobSeekerRepositories;
using CareerCompassAPI.Application.Abstraction.Repositories.IVacancyRepositories;
using CareerCompassAPI.Application.DTOs.Application_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using CareerCompassAPI.Persistence.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CareerCompassAPI.Tests
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IJobSeekerReadRepository> _jobSeekerReadRepositoryMock = new();
        private readonly Mock<IVacancyReadRepository> _vacancyReadRepositoryMock = new();
        private readonly Mock<IJobApplicationWriteRepository> _jobApplicationWriteRepositoryMock = new();
        private readonly Mock<IVacancyWriteRepository> _vacancyWriteRepositoryMock = new();
        private readonly Mock<IJobApplicationReadRepository> _jobApplicationReadRepositoryMock = new();
        private readonly Mock<ILogger<ApplicationService>> _loggerMock = new();
        private readonly Mock<ICareerCompassDbContext> _contextMock = new();

        private ApplicationService CreateService(ICareerCompassDbContext context)
        {
            return new ApplicationService(
                context,
                _jobSeekerReadRepositoryMock.Object,
                _vacancyReadRepositoryMock.Object,
                _jobApplicationWriteRepositoryMock.Object,
                _vacancyWriteRepositoryMock.Object,
                _jobApplicationReadRepositoryMock.Object,
                _loggerMock.Object
            );
        }


        [Fact]
        public async Task CreateAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<CareerCompassDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_WhenDtoIsNull")
                .Options;

            using (var context = new CareerCompassDbContext(options))
            {
                var service = CreateService(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(null));
            }
        }

        [Fact]
        public async Task CreateAsync_WhenVacancyDoesNotExist_ThrowsNotFoundException()
        {
            var options = new DbContextOptionsBuilder<CareerCompassDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_WhenVacancyDoesNotExist")
                .Options;

            using (var context = new CareerCompassDbContext(options))
            {
                var service = CreateService(context);
                var dto = new ApplicationCreateDto(Guid.NewGuid(), Guid.NewGuid());

                _vacancyReadRepositoryMock.Setup(v => v.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Vacancy)null);

                await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(dto));
            }
        }

        [Fact]
        public async Task CreateAsync_WhenApplicationLimitReached_ThrowsLimitExceededException()
        {
            var options = new DbContextOptionsBuilder<CareerCompassDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_WhenApplicationLimitReached")
                .Options;

            using (var context = new CareerCompassDbContext(options))
            {
                var service = CreateService(context);
                var dto = new ApplicationCreateDto(Guid.NewGuid(), Guid.NewGuid());
                var vacancy = new Vacancy { CurrentApplicationCount = 10, ApplicationLimit = 10 };

                _vacancyReadRepositoryMock.Setup(v => v.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(vacancy);

                await Assert.ThrowsAsync<LimitExceededException>(() => service.CreateAsync(dto));
            }
        }

        [Fact]
        public async Task CreateAsync_WhenJobSeekerDoesNotExist_ThrowsNotFoundException()
        {
            var options = new DbContextOptionsBuilder<CareerCompassDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_WhenJobSeekerDoesNotExist_ThrowsNotFoundException")
                .Options;

            using (var context = new CareerCompassDbContext(options))
            {
                var service = CreateService(context);
                var dto = new ApplicationCreateDto(Guid.NewGuid(), Guid.NewGuid());
                var vacancy = new Vacancy { CurrentApplicationCount = 0, ApplicationLimit = 10 };

                _vacancyReadRepositoryMock.Setup(v => v.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(vacancy);
                _jobSeekerReadRepositoryMock.Setup(j => j.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((JobSeeker)null);

                await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(dto));
            }
        }

        [Fact]
        public async Task CreateAsync_HappyPath_ReturnsNewApplicationCount()
        {
            var options = new DbContextOptionsBuilder<CareerCompassDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_HappyPath_ReturnsNewApplicationCount")
                .Options;

            using (var context = new CareerCompassDbContext(options))
            {
                var service = CreateService(context);
                var dto = new ApplicationCreateDto(Guid.NewGuid(), Guid.NewGuid());
                var vacancy = new Vacancy { CurrentApplicationCount = 0, ApplicationLimit = 10 };
                var jobSeeker = new JobSeeker { Id = Guid.NewGuid() };

                _vacancyReadRepositoryMock.Setup(v => v.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(vacancy);
                _jobSeekerReadRepositoryMock.Setup(j => j.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(jobSeeker);

                int result = await service.CreateAsync(dto);

                Assert.Equal(1, result);
            }
        }

    }
}
