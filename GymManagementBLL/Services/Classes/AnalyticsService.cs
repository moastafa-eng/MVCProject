using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {

            var SessionRepository = _unitOfWork.GetRepository<Session>();

            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = SessionRepository.GetAll(x => x.StartDate > DateTime.UtcNow).Count(),
                OngoingSessions = SessionRepository.GetAll(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow).Count(),
                CompletedSessions = SessionRepository.GetAll(x => x.EndDate < DateTime.Now).Count()
            };
        }
    }
}
