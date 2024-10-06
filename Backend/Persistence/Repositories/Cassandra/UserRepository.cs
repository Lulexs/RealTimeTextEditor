using System.Linq.Expressions;
using Cassandra.Mapping;
using Models;
using Persistence.Interfaces;

namespace Persistence.Repositories.Cassandra;

public class UserRepository : IAsyncAccess<User>
{
    public async Task<User?> GetOneByIdAsync(Guid Id) {
        var session = SessionManager.GetSession();
        var mapper = new Mapper(session);

        User user = await mapper.SingleOrDefaultAsync<User>("SELECT user_id, username, password FROM user WHERE user_id = ?", Id);

        return user;
    }

    public async Task<User?> GetOneByPropertyAsync(string propertyName, string propertyValue) {
        var session = SessionManager.GetSession();
        var mapper = new Mapper(session);
        
        var userMap = MappingConfiguration.Global.Get<User>();
        var columnName = userMap.GetColumnDefinition(typeof(User).GetProperty(propertyName)).ColumnName;

        User user = await mapper.SingleOrDefaultAsync<User>(
            $"SELECT user_id, username, password FROM users_by_{columnName} WHERE {columnName} = ?", propertyValue);

        return user;
    }

    public Task<bool> DeleteOneByIdAsync(Guid Id) {
        throw new NotImplementedException();
    }

    public Task<User> InsertOneAsync<DtoT>(DtoT dto)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateOneAsync<DtoT>(DtoT dto)
    {
        throw new NotImplementedException();
    }
}