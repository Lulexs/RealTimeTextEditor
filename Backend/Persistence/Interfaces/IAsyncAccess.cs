using System.Linq.Expressions;

namespace Persistence.Interfaces;

public interface IAsyncAccess<T> {
    Task<T?> GetOneByIdAsync(Guid Id);
    Task<T?> GetOneByProperty(Expression<Func<T, bool>> predicate);
    Task<T> InsertOneAsync<DtoT>(DtoT dto);
    Task<bool> DeleteOneByIdAsync(Guid Id);
    Task<T> UpdateOneAsync<DtoT>(DtoT dto);
    // Task<IEnumerable<P>> GetManyByPropertyAsync<T, P>(); //TODO
}