using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels
{
    // This ViewModel Represents The Data Structure For Displaying Member Information
    // Instead of Exposing The Entire Member Entity Directly it works as a Data Transfer Object (DTO)
    // between DAL and BLL Layers.
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Photo { get; set; }
        public string? PlanName { get; set; } = null;
        public string? MembershipStartDate { get; set; } = null;
        public string? MembershipEndDate { get; set; } = null;

    }
}
