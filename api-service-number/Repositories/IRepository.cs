using api_service_number.Models;
using api_service_number.Services;

namespace api_service_number.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> CreateAsync(T entity);
}