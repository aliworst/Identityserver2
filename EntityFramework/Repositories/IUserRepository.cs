using DataAccess.RequestModel;
using EntityFramework.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFramework.Repositories;

public interface IUserRepository
{
    Task<EntityEntry<Users>> CreateUserAsync(Users user);
    Task<Users?> GetUserAsync(string userId);
    Task<int> RemoveUserAsync(Users user);
    Task<int> UpdateUsersAsync(Users user);
}