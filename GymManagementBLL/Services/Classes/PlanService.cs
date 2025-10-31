using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Activate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan == null || HasActiveMemberShip(planId))
                return false;

            plan.IsActive = !plan.IsActive; // if it true [not true false], if it false [not false true].
            plan.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GetRepository<Plan>().Update(plan);
            return _unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll() ?? [];

            if (plans == null || !plans.Any())
                return [];

            var planViewModel = plans.Select(x => new PlanViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                DurationDays = x.DurationDays,
                Price = x.Price,
                IsActive = x.IsActive,
            });

            return planViewModel;
        }

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan == null)
                return null;

            var planViewModel = new PlanViewModel
            {
                Id = planId,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive,
            };

            return planViewModel;
        }

        public PlanToUpdateViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan == null || plan.IsActive == false) // Update only active plans.
                return null;

            var planToUpdateViewModel = new PlanToUpdateViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
            };
            return planToUpdateViewModel;
        }

        public bool UpdatePlan(int planId, PlanToUpdateViewModel updatePlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);

            if (plan == null || HasActiveMemberShip(planId))
                return false;

            try
            {
                plan.Name = updatePlan.PlanName;
                plan.Description = updatePlan.Description;
                plan.DurationDays = updatePlan.DurationDays;
                plan.Price = updatePlan.Price;

                // Tuple Way to update plan
                //(plan.Name, plan.Description, plan.DurationDays, plan.Price) =
                //    (updatePlan.PlanName, updatePlan.Description, updatePlan.DurationDays, updatePlan.Price);

                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0; // return true of false.
            }
            catch
            {
                return false;
            }

         
        }

        #region Helper methods
        
        private bool HasActiveMemberShip(int planId)
        {
            return _unitOfWork.GetRepository<MemberShip>()
                .GetAll(x => x.PlanId == planId && x.Status == "Active")
                .Any();
                
        }

        #endregion
    }
}
