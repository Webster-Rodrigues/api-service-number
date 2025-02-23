using api_service_number.Context;
using api_service_number.Models;
using api_service_number.Services;
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
       return context.Set<T>().AsNoTracking().ToList();
    }

    public T? GetById(int id)
    {
        return context.Set<T>().Find(id);
    }
    
    public T Create(T entity)
    {
        context.Set<T>().Add(entity);
        context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        context.Set<T>().Update(entity);
        context.SaveChanges();
        return entity;
    }

    public T Delete(T entity)
    {
        context.Set<T>().Remove(entity);
        context.SaveChanges();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        await context.SaveChangesAsync(); 
        return entity;
    }
}