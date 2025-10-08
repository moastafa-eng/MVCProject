using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.interfaces
{
    public interface IMemberRepository
    {
        Member? GetById(int Id);
        IEnumerable<Member> GetAll();
        int Add(Member member);
        int Update(Member member);
        int Delete(int Id);

    }
}
