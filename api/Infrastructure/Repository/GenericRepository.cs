using System.Linq.Expressions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply eager loading for specified navigation properties
        if (includes != null)
        {
            foreach (var navigationProperty in includes)
            {
                if (!string.IsNullOrWhiteSpace(navigationProperty))
                    query = query.Include(navigationProperty);
            }
        }

        // Apply additional predicate if provided
        if (predicate != null)
            query = query.Where(predicate);
        
        return await query.ToListAsync();
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        // Apply eager loading for specified navigation properties
        if (includes != null)
        {
            foreach (var navigationProperty in includes)
            {
                if (!string.IsNullOrWhiteSpace(navigationProperty))
                    query = query.Include(navigationProperty);
            }
        }

        query = query.Where(predicate);
        return query.FirstOrDefaultAsync();
    }
    public async Task<T?> FindAsync(object id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        return entity;
    }
    public async Task<T> AddAsync(T entity)
    {
        if (entity != null)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        if (entities != null)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities ;
        }
        throw new ArgumentNullException(nameof(entities), "Entities cannot be null");
    }
    public Task<T> Update(T entity)
    {
        if (entity != null)
        {
            var result = _context.Set<T>().Update(entity);
            return Task.FromResult(result.Entity);
        }
        throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
    }

    public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var existingEntity = await _context.Set<T>().FirstOrDefaultAsync(predicate);

        if (existingEntity != null)
        {
            _context.Set<T>().Remove(existingEntity);
            return true ;
        }
        return false ;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AnyAsync(predicate);
    }
    public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return predicate == null ?
            _context.Set<T>().CountAsync() :
            _context.Set<T>().CountAsync(predicate);
    }



}