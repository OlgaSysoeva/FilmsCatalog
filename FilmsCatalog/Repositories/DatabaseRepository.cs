using FilmsCatalog.Data;
using FilmsCatalog.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsCatalog.Repositories
{
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : class
    {
        protected CatalogContext CatalogContext { get; }

        public DatabaseRepository(CatalogContext context)
        {
            CatalogContext = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            CatalogContext.Set<T>().Add(entity);
            await CatalogContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            CatalogContext.Set<T>().Remove(entity);
            await CatalogContext.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await CatalogContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await CatalogContext.Set<T>().ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            CatalogContext.Entry(entity).State = EntityState.Modified;
            await CatalogContext.SaveChangesAsync();
        }
    }
}
