using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly GymDbContext _context;
        public TrainerRepository(GymDbContext context)
        {
            _context = context;
        }
        public int Add(Trainer trainer)
        {
            _context.Add(trainer);
            return _context.SaveChanges();
        }

        public int Delete(int Id)
        {
            var trainer = GetById(Id);

            if (trainer is null)
                return 0;

            _context.Remove(trainer);
            return _context.SaveChanges();
        }

        public IEnumerable<Trainer> GetAll() => _context.Trainers.ToList();

        public Trainer? GetById(int Id) => _context.Trainers.Find(Id);


        public int Update(Trainer trainer)
        {
            _context.Update(trainer);
            return _context.SaveChanges();
        }
    }
}
