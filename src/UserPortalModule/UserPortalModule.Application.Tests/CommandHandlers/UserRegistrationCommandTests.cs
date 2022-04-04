using AutoFixture;
using AutoFixture.AutoMoq;
using UserPortalModule.Common;
using CoreModule.Domain.Users;
using MockQueryable.Moq;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UserPortalModule.CommandHandlers;

public class UserRegistrationCommandTests : TestBase
{
    [Fact]
    public async Task CorrectData_CreateUser_Success()
    {
        var fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        var command = fixture.Create<UserRegistrationCommand>();

        var users = fixture.CreateMany<User>(5);

        var mockUserDbSet = users.AsQueryable().BuildMockDbSet();

        mockUserDbSet.Setup(x => x.Add(It.IsAny<User>()));

        Mock<IUserPortalModuleDbContext> mockDbContext = new Mock<IUserPortalModuleDbContext>();

        mockDbContext.Setup(x => x.Users).Returns(mockUserDbSet.Object);

        var handler = new UserRegistrationCommand.Handler(mockDbContext.Object, Mapper);

        var result = await handler.Handle(command);

        Assert.True(result.Succeeded);
    }
}

