using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        // Get Reference from DbContext
        public readonly GymDbContext _context;

        //Inject DbContext via constructor
        public GenericRepository(GymDbContext context)
        {
            _context = context;
        }
        public void Add(TEntity entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        // Func<TEntity, bool>? Condition = null: optional condition (filter) — you can pass a function to filter the data or leave it empty to get all data
        // _context.Set<TEntity>(): used to dynamically access the table that matches the current entity type (TEntity)
        // AsNoTracking(): makes the query faster because EF won’t track the entities for changes (used for read-only operations)
        // Where(Condition): applies the filter if it exists
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? Condition = null)
        {
            // if a condition (filter) is passed, return only the filtered data
            if (Condition is not null)
            {
                return _context.Set<TEntity>() // dynamically get the correct DbSet (table)
                    .AsNoTracking(). // improve performance
                    Where(Condition).ToList();
            }
            else
            {
                return _context.Set<TEntity>().AsNoTracking().ToList();
            }
        }

        public TEntity? GetById(int Id) => _context.Set<TEntity>().Find(Id);

        public void Update(TEntity entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
