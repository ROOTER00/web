using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kurdixane.Web.Data;
using Kurdixane.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Kurdixane.Web.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> Set;

    public Repository(ApplicationDbContext context)
    {
        Context = context;
        Set = context.Set<T>();
    }

    public IQueryable<T> Query(bool tracking = false)
        => tracking ? Set : Set.AsNoTracking();

    public async Task<List<T>> GetAllAsync()
        => await Set.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
        => await Set.FindAsync(id);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        => await Set.AsNoTracking().FirstOrDefaultAsync(predicate);

    public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        => await Set.AsNoTracking().Where(predicate).ToListAsync();

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await Set.AnyAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        => predicate is null ? await Set.CountAsync() : await Set.CountAsync(predicate);

    public async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await Set.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        Set.Update(entity);
    }

    public void Remove(T entity) => Set.Remove(entity);

    public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
}
