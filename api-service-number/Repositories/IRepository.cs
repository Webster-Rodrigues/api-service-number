using api_service_number.Models;

namespace api_service_number.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    T Add(T entity);
    T Update(T entity);
    T Delete(T entity);
    
}