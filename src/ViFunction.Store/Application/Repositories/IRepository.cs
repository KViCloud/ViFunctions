namespace ViFunction.Store.Application.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task SaveChangesAsync();
}