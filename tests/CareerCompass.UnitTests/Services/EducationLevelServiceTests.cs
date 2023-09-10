using AutoMapper;
using CareerCompassAPI.Application.DTOs.EducationLevel_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Implementations.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CareerCompass.UnitTests.Services
{
    public class EducationLevelServiceTests
    {
        [Fact]
        public async Task GetAllAsync_WhenEducationLevelsExist_ReturnsEducationLevelGetDtos()
        {
            var educationLevels = new List<EducationLevel>
            {
                new EducationLevel { Id = Guid.NewGuid(), Name = "Bachelor's Degree" },
                new EducationLevel { Id = Guid.NewGuid(), Name = "Master's Degree" },
                new EducationLevel { Id = Guid.NewGuid(), Name = "Ph.D." },
            };

            var options = new DbContextOptionsBuilder<CareerCompassDbContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            using var context = new CareerCompassDbContext(options);
            context.EducationLevels.AddRange(educationLevels);
            context.SaveChanges();

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<List<EducationLevelGetDto>>(educationLevels))
                .Returns(educationLevels.Select(el => new EducationLevelGetDto(el.Id, el.Name)).ToList());

            var service = new EducationLevelService(context, mapperMock.Object);
            var result = await service.GetAllAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(educationLevels.Count, result.Count);
        }
    }
}
