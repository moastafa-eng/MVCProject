using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; // Inject IMapper through constructor
        }
        public bool CreateSession(CreateSessionViewModel model)
        {
            if (!IsTrainerExist(model.TrainerId))
                return false;

            if (!IsCategoryExist(model.CategoryId))
                return false;

            if(!IsValidDateRange(model.StartDate, model.EndDate))
                return false;

            var sessionEntity = _mapper.Map<CreateSessionViewModel, Session>(model); // Map CreateSessionViewModel to Session entity

            _unitOfWork.GetRepository<Session>().Add(sessionEntity); // Add new Session entity to repository
            
            return _unitOfWork.SaveChanges() > 0; // Save changes and return true if successful
        }

        public SessionToUpdateViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if(session is null)
            {
                return null;
            }

            return _mapper.Map<Session,  SessionToUpdateViewModel>(session);
        }
        public bool UpdateSession(int sessionId, SessionToUpdateViewModel model)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if (!IsSessionAvailableFroUpdate(session))
                return false;

            if (!IsTrainerExist(model.TrainerId))
                return false;

            if (!IsValidDateRange(model.StartDate, model.EndDate))
                return false;

            var updatedSession = _mapper.Map<SessionToUpdateViewModel, Session>(model);
            updatedSession.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Session>().Update(updatedSession);

            return _unitOfWork.SaveChanges() > 0;
        }

        public bool RemoveSession(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

            if(!IsSessionAvailableFroDelete(session))
            {
                return false;
            }

            _unitOfWork.GetRepository<Session>().Delete(session);

            return _unitOfWork.SaveChanges() > 0;
        }
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository
                .GetAllSessionsWithTrainerAndCategory() 
                .OrderByDescending(s => s.StartDate); // Order by StartDate descending

            if (sessions is null || !sessions.Any())
                return [];

            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(sessions); // Map Session entities to SessionViewModel

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id); // Calculate AvailableSlots
            }

            return MappedSessions;
        }


        public SessionViewModel? GetSessionDetails(int sessionId)
        {
            var session = _unitOfWork.SessionRepository
            .GetSessionWithTrainerAndCategory(sessionId);

            if (session is null)
                return null;


            var mappedSession = _mapper.Map<Session, SessionViewModel>(session); // Map Session entity to SessionViewModel
            mappedSession.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(mappedSession.Id); // Calculate AvailableSlots

            return mappedSession;
        }

        


        #region Helper Methods 
        private bool IsTrainerExist(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }

        private bool IsCategoryExist(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }

        private bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate > DateTime.UtcNow;
        }

        private bool IsSessionAvailableFroUpdate(Session session)
        {
            if (session is null)
                return false;

            if (session.EndDate < DateTime.UtcNow)
                return false;

            if (session.StartDate <= DateTime.UtcNow) 
                return false;

            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (hasActiveBookings)
                return false;

            return true;
        }

        private bool IsSessionAvailableFroDelete(Session session)
        {
            if (session is null)
                return false;

            if (session.StartDate > DateTime.UtcNow)
                return false;

            if (session.StartDate <= DateTime.UtcNow && session.EndDate > DateTime.UtcNow)
                return false;

            var hasActiveBookings = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (hasActiveBookings)
                return false;

            return true;
        }

        #endregion
    }
}
