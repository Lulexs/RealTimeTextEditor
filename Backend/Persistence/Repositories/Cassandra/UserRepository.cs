using System.Linq.Expressions;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using Models;
using Persistence.Interfaces;

namespace Persistence.Repositories.Cassandra;

public class UserRepository : IAsyncAccess<User>
{
    public Task<bool> DeleteOneByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetOneByIdAsync(Guid Id) {
        var session = SessionManager.GetSession();
        var mapper = new Mapper(session);

        User user = await mapper.SingleOrDefaultAsync<User>("SELECT user_id, username, password FROM user WHERE user_id = ?", Id);

        return user;
    }

    public async Task<User?> GetOneByProperty(Expression<Func<User, bool>> predicate) {
        var session = SessionManager.GetSession();

        var table = new Table<User>(session, MappingConfiguration.Global);

        var user = await table.FirstOrDefault(predicate).ExecuteAsync();

        return user;
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