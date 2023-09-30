using EntityFramework.DbModels;

namespace BusinessRule.Services;

public interface IUsersServices
{
    Task<Users> CreateUserAsync(Users user);
    Task<Users?> GetUserAsync(string userId);
    Task<int> RemoveUserAsync(Users user);
    Task<int> UpdateUsersAsync(Users user);
}