using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.interfaces
{
    public interface ITrainerRepository
    {
        Trainer? GetById(int Id);
        IEnumerable<Trainer> GetAll();
        int Add(Trainer trainer);
        int Update(Trainer trainer);
        int Delete(int Id);
    }
}
