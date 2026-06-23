using Microsoft.AspNetCore.Identity;
using Moq;

namespace CodeJudex.Identity.UnitTests;

/// <summary>
/// Provides helper methods to create mocks for ASP.NET Core Identity managers.
/// </summary>
public static class MockHelpers
{
    /// <summary>
    /// Creates a mock of <see cref="UserManager{TUser}"/>.
    /// </summary>
    public static Mock<UserManager<TUser>> GetUserManagerMock<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    /// <summary>
    /// Creates a mock of <see cref="RoleManager{TRole}"/>.
    /// </summary>
    public static Mock<RoleManager<TRole>> GetRoleManagerMock<TRole>() where TRole : class
    {
        var store = new Mock<IRoleStore<TRole>>();
        return new Mock<RoleManager<TRole>>(
            store.Object, null!, null!, null!, null!);
    }
}