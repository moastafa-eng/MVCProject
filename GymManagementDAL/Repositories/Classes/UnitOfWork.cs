using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;

namespace GymManagementDAL.Repositories.Classes
{

    // The UnitOfWork class is responsible for managing all repositories using the same DbContext instance.
    // It ensures that multiple operations (like Add, Update, Delete) are done within one transaction.
    // We call SaveChanges() only once at the end to save all changes together, keeping the data consistent.
    // Each entity that is added, updated, or deleted becomes "tracked" by the DbContext.
    // The DbContext keeps track of every change in these entities (their state: Added, Modified, Deleted, etc.).
    // When SaveChanges() is called, all tracked changes are written to the database,
    // and the entity states are set to Unchanged because the data is now synchronized with the database.
    // When the request ends, the DbContext is automatically disposed and a new one is created for the next request.
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context; // DbContext instance shared across repositories
        private readonly Dictionary<string, object> Repositories = []; // Stores created generic repositories to avoid re-creation
        public ISessionRepository SessionRepository { get; set; } // this Field is public to be accessed from outside

        public UnitOfWork(GymDbContext context, ISessionRepository sessionRepository) 
        {
            _context = context; // Inject DbContext through constructor
            SessionRepository = sessionRepository; // Inject specific repository through constructor
        }


        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var EntityName = typeof(TEntity).Name;
            if (Repositories.TryGetValue(EntityName, out object? value)) 
                return (IGenericRepository<TEntity>)value; // Explicit casting from object to [IGenericRepository<TEntity>]

            var Repository = new GenericRepository<TEntity>(_context);// Pass _context as an initial value (dependency flow)
            Repositories.Add(EntityName, Repository); // Add the new generic repository to the dictionary

            return Repository;
        }

        public int SaveChanges() // Save all tracked changes in one transaction
        =>  _context.SaveChanges();
    }
}
