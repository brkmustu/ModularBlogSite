using AutoFixture;
using AutoFixture.AutoMoq;
using ManagementModule.Common;
using CoreModule.Application.Common.Contracts;
using CoreModule.Domain.Users;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ManagementModule.QueryHandlers;

public class GetUserListQueryTests : TestBase
{
    [Fact]
    public async Task OnlyActive_GetUsers_Success()
    {
        var fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        var pageInfo = fixture.Freeze<PageInfo>();
        pageInfo.PageSize = 10;
        pageInfo.PageIndex = 0;

        var query = fixture.Freeze<GetUserListQuery>();

        query.UserStatusId = (int?)null;
        query.IsActive = true;
        query.Paging = pageInfo;

        var activeUsers = fixture.CreateMany<User>(5);
        var passiveUsers = fixture.CreateMany<User>(3);
        var users = new List<User>();
        
        foreach (var item in activeUsers)
        {
            item.Activate();
            users.Add(item);
        }
        foreach (var item in passiveUsers)
        {
            item.Deactivate();
            users.Add(item);
        }

        var mockUserDbSet = users.AsQueryable().BuildMockDbSet();

        mockUserDbSet.Setup(x => x.Add(It.IsAny<User>()));

        Mock<IManagementModuleDbContext> mockDbContext = new Mock<IManagementModuleDbContext>();

        mockDbContext.Setup(x => x.Users).Returns(mockUserDbSet.Object);

        var handler = new GetUserListQuery.Handler(mockDbContext.Object, Mapper);

        var response = await handler.Handle(query);

        //Assert.Equal(response.Users.Items.Length, activeUsers.Count());
    }
}

