using AutoFixture;
using AutoFixture.AutoMoq;
using ManagementModule.Common;
using CoreModule.Domain.Users;
using MockQueryable.Moq;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ManagementModule.CommandHandlers;

public class UserActivationStatusCommandTests : TestBase
{
    [Fact]
    public async Task CorrectData_ActivateUser_Success()
    {
        var fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        var command = fixture.Create<UserActivationStatusCommand>();

        var users = fixture.CreateMany<User>(5).ToList();

        var sutUser = users.FirstOrDefault();

        users.Remove(sutUser);

        sutUser.Id = command.UserId;

        users.Add(sutUser);

        var mockUserDbSet = users.AsQueryable().BuildMockDbSet();

        mockUserDbSet.Setup(x => x.Add(It.IsAny<User>()));

        Mock<IManagementModuleDbContext> mockDbContext = new Mock<IManagementModuleDbContext>();

        mockDbContext.Setup(x => x.Users).Returns(mockUserDbSet.Object);

        var handler = new UserActivationStatusCommand.Handler(mockDbContext.Object, Mapper);

        var result = await handler.Handle(command);

        Assert.True(result.Succeeded);
    }
}

