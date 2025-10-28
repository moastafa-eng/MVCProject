﻿using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            // Eager loading Trainer and Category related entities
            return _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _context.Bookings.Where(x => x.SessionId == sessionId).Count(); // Count bookings for the specified session
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefault(x => x.Id == sessionId);
        }
    }
}
