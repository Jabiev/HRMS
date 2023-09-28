using CareerCompassAPI.Application.Abstraction.Repositories.ITeamRepositories;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Exceptions;
using CareerCompassAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace CareerCompassAPI.Tests
{
    public class TeamMemberServiceTests
    {
        private readonly Mock<ITeamReadRepository> _mockRepo;
        private readonly TeamMemberService _service;

        public TeamMemberServiceTests()
        {
            _mockRepo = new Mock<ITeamReadRepository>();
            _service = new TeamMemberService(_mockRepo.Object);
        }

        //[Fact]
        //public async Task GetMembers_ReturnsListOfTeamMembers()
        //{
        //    Guid id1 = Guid.NewGuid();
        //    Guid id2 = Guid.NewGuid();
        //    var fakeMembers = new List<TeamMember>
        //    {
        //        new TeamMember { Id = id1, FirstName = "John", LastName = "Doe", IsDeleted = false },
        //        new TeamMember { Id = id2, FirstName = "Jane", LastName = "Doe", IsDeleted = false },
        //    };

        //    _mockRepo.Setup(repo => repo.GetAllByExpression(
        //      It.IsAny<Expression<Func<TeamMember, bool>>>(),
        //      It.IsAny<int>(),
        //      It.IsAny<int>(),
        //      It.IsAny<bool>(),
        //      It.IsAny<string[]>()
        //  )).Returns(fakeMembers.AsQueryable());

        //    _mockRepo.Setup(repo => repo.GetAllByExpression(
        //        It.IsAny<Expression<Func<TeamMember, bool>>>(),
        //        It.IsAny<int>(),
        //        It.IsAny<int>(),
        //        It.IsAny<bool>(),
        //        It.IsAny<string[]>()
        //    ).ToListAsync()).Returns(Task.FromResult(fakeMembers));

        //    var result = await _service.GetMembers();

        //    Assert.Equal(2, result.Count);
        //    Assert.Equal(id1, result[0].memberId);
        //    Assert.Equal(id2, result[1].memberId);
        //    Assert.Equal("John", result[0].firstName);
        //    Assert.Equal("Jane", result[1].firstName);
        //}

        [Fact]
        public async Task GetMembers_ThrowsNotFoundException()
        {
            _mockRepo.Setup(repo => repo.GetAllByExpression(
                It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>(),
                It.IsAny<string[]>()
            )).Returns((IQueryable<TeamMember>)Enumerable.Empty<TeamMember>().AsQueryable().ToAsyncEnumerable());

            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetMembers());
        }
    }
}
