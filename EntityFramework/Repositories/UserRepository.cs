using DataAccess.RequestModel;
using Duende.IdentityServer.Models;
using EntityFramework.DbContexts;
using EntityFramework.DbModels;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFramework.Repositories;

public class UserRepository : IUserRepository
{
    protected readonly UserDbContext UsersdbContexts;
    public UserRepository(UserDbContext usersDbContexts)
    {
        UsersdbContexts = usersDbContexts;
    }
    
    public virtual async Task<EntityEntry<Users>> CreateUserAsync(Users user)
    {
        user.PasswordHash = user.PasswordHash.ToSha256();
        var identityResult = await UsersdbContexts.IdentityUsers.AddAsync(user);
        await UsersdbContexts.SaveChangesAsync();
        return identityResult;
    }

    public virtual Task<Users?> GetUserAsync(string userId)
    {
        return UsersdbContexts.IdentityUsers
            .Where(x => x.Id == userId)
            .AsNoTracking()
            .SingleOrDefaultAsync();
        
    }
    
    public virtual async Task<int> RemoveUserAsync(Users user)
    {
        UsersdbContexts.IdentityUsers.Remove(user);

        return await UsersdbContexts.SaveChangesAsync();
    }
    
    public virtual async Task<int> UpdateUsersAsync(Users user)
    {
        UsersdbContexts.IdentityUsers.Update(user);
        
        return await UsersdbContexts.SaveChangesAsync();
    }
}