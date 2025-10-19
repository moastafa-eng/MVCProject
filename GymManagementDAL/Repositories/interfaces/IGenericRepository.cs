using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity? GetById(int Id);

        // This method returns a collection of all entities from the database.
        // You can optionally pass a condition (Func<TEntity, bool>) to filter the results.
        // - If 'Condition' is null → it returns all records.
        // - If 'Condition' is provided → it returns only records that match the condition.
        // Example:
        //   GetAll();                           // returns all records
        //   GetAll(x => x.Name == "Sport");     // returns filtered records
        IEnumerable<TEntity> GetAll(Func<TEntity, bool>? Condition = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
