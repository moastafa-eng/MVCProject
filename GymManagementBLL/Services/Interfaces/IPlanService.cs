using GymManagementBLL.ViewModels.PlanViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int planId);
        PlanToUpdateViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlan(int planId, PlanToUpdateViewModel updatePlan);
        bool Activate(int planId);
    }
}
