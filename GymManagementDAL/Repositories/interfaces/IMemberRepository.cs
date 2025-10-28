using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        IEnumerable<Session> GetAllSessions(int MemberId);

    }
}
