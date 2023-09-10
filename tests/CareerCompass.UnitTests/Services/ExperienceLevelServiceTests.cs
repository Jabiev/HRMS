using AutoMapper;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using CareerCompassAPI.Persistence.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CareerCompass.UnitTests.Services
{
    public class ExperienceLevelServiceTests
    {
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<ILogger<ExperienceLevelService>> _loggerMock = new Mock<ILogger<ExperienceLevelService>>();
        private readonly Mock<ICareerCompassDbContext> _contextMock = new Mock<ICareerCompassDbContext>();


        [Fact]
        public async Task GetAllAsync_WhenNoExperienceLevelsExist_ThrowsNotFoundException()
        {
            
            var service = new ExperienceLevelService(_mapperMock.Object, _contextMock.Object);

            var experienceLevels = Enumerable.Empty<ExperienceLevel>().AsQueryable();

            var mockSet = SetupMockDbSet(experienceLevels);

            _contextMock.Setup(c => c.ExperienceLevels).Returns(mockSet.Object);

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetAllAsync());
        }

        private Mock<DbSet<ExperienceLevel>> SetupMockDbSet(IQueryable<ExperienceLevel> experienceLevels)
        {
            var mockSet = new Mock<DbSet<ExperienceLevel>>();
            mockSet.As<IAsyncEnumerable<ExperienceLevel>>()
                .Setup(d => d.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new TestAsyncEnumerator<ExperienceLevel>(experienceLevels.GetEnumerator()));

            mockSet.As<IQueryable<ExperienceLevel>>().Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<ExperienceLevel>(experienceLevels.Provider));

            mockSet.As<IQueryable<ExperienceLevel>>().Setup(m => m.Expression).Returns(experienceLevels.Expression);
            mockSet.As<IQueryable<ExperienceLevel>>().Setup(m => m.ElementType).Returns(experienceLevels.ElementType);
            mockSet.As<IQueryable<ExperienceLevel>>().Setup(m => m.GetEnumerator()).Returns(experienceLevels.GetEnumerator());

            return mockSet;
        }
    }
}
