using api_service_number.Context;
using Microsoft.EntityFrameworkCore;

namespace api_service_number.Repositories;

public class Repository<T> :IRepository<T> where T : class
{
    protected readonly AppDbContext context;

    public Repository(AppDbContext context)
    {
        this.context = context;
    }
    
    
    public IEnumerable<T> GetAll()
    {
        context.Tickets.Load();
       return context.Set<T>().AsNoTracking().ToList();
    }

    public T? GetById(int id)
    {
        return context.Set<T>().Find(id);
    }

    public T Add(T entity)
    {
        return context.Set<T>().Add(entity).Entity;
    }

    public T Update(T entity)
    {
        return context.Set<T>().Update(entity).Entity;
    }

    public T Delete(T entity)
    {
        return context.Set<T>().Remove(entity).Entity;
    }
}