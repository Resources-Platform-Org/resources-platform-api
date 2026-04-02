using System.Linq.Expressions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate, bool trackChanges = false, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        // Configure tracking behavior
        if (!trackChanges)
            query = query.AsNoTracking();

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
    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, bool trackChanges = false, params string[]? include)
    {
        IQueryable<T> query = _context.Set<T>();

        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync();

        // Configure tracking behavior
        if (!trackChanges)
            query = query.AsNoTracking();

        // Apply eager loading for specified navigation properties
        if (include != null)
        {
            foreach (var navigationProperty in include)
            {
                if (!string.IsNullOrWhiteSpace(navigationProperty))
                    query = query.Include(navigationProperty);
            }
        }

        var keyProperty = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.FirstOrDefault();
        if (keyProperty != null)
        {
            query = query.OrderBy(e => EF.Property<object>(e, keyProperty.Name));
        }

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        // Configure tracking behavior
        if (!trackChanges)
            query = query.AsNoTracking();

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
    public async Task AddAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities), "Entities cannot be null");
        }
        await _context.Set<T>().AddRangeAsync(entities);
    }
    public void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }
        _context.Set<T>().Update(entity);
    }
    public void Remove(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }
        _context.Set<T>().Remove(entity);
    }

    public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate)
    {
        var rowAffected = await _context.Set<T>()
            .Where(predicate)
            .ExecuteDeleteAsync();

        return rowAffected > 0;
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