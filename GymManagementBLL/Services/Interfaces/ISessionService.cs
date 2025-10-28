using GymManagementBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionDetails(int sessionId);
        bool CreateSession(CreateSessionViewModel model);
        bool UpdateSession(int sessionId, SessionToUpdateViewModel model);

        SessionToUpdateViewModel? GetSessionToUpdate(int sessionId);
        bool RemoveSession(int sessionId);

    }
}
