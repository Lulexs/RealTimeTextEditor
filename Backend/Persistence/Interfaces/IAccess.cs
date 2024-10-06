namespace Persistence.Interfaces;

public interface IAsyncAccess {
    Task<T> GetOneByIdAsync<T>(Guid Id); 
    Task<T> InsertOneAsync<T, DtoT>(DtoT dto);
    Task<bool> DeleteOneByIdAsync<T>(Guid Id);
    Task<T> UpdateOneAsync<T, DtoT>(DtoT dto);
    Task<IEnumerable<T>> GetManyByPropertyAsync<T>(); //TODO
}