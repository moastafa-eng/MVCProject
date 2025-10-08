using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context)
        {
            _context = context;
        }
        public int Add(Member member)
        {
            _context.Add(member);
            return _context.SaveChanges();
        }

        public int Delete(int Id)
        {
            var member = GetById(Id);

            if (member is null)
                return 0;
            
            _context.Remove(member);
            return _context.SaveChanges();
        }

        public IEnumerable<Member> GetAll() => _context.Members.ToList();
        
        public Member? GetById(int Id) => _context.Members.Find(Id);
     

        public int Update(Member member)
        {
            _context.Update(member);
            return _context.SaveChanges();
        }
    }
}
