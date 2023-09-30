using EntityFramework.DbModels;
using EntityFramework.Repositories;

namespace BusinessRule.Services;

public class UsersServices : IUsersServices
{
    protected readonly IUserRepository UserRepository;
    
    public UsersServices(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }
    public virtual async Task<Users> CreateUserAsync(Users user)
    {
        var insertedUser = await UserRepository.CreateUserAsync(user);
        var responseUser = new Users
        {
            Id = insertedUser.Entity.Id,
            Enabled = insertedUser.Entity.Enabled,
            UserName = insertedUser.Entity.UserName,
            Forename = insertedUser.Entity.Forename,
            Surname = insertedUser.Entity.Surname,
            NationalIdentifier = insertedUser.Entity.NationalIdentifier,
            LockoutEnabled = insertedUser.Entity.LockoutEnabled,
            ConcurrencyStamp = insertedUser.Entity.ConcurrencyStamp,
            SecurityStamp = insertedUser.Entity.SecurityStamp,
            TwoFactorEnabled = insertedUser.Entity.TwoFactorEnabled,
            AccessFailedCount = insertedUser.Entity.AccessFailedCount,
            LockoutEnd = insertedUser.Entity.LockoutEnd
        };
        return (responseUser);
    }

    public async Task<Users?> GetUserAsync(string userId)
    {
        Users? user = await UserRepository.GetUserAsync(userId);
        
        return user;
    }
    
    public virtual async Task<int> RemoveUserAsync(Users user)
    {
        var deleted = await UserRepository.RemoveUserAsync(user);

        return deleted;
    }
    
    public async Task<int> UpdateUsersAsync(Users user)
    {
        var updated = await UserRepository.UpdateUsersAsync(user);

        return updated;
    }
}