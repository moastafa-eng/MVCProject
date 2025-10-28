using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionViewModel
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TrainerName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public int AvailableSlots { get; set; }

        // computed Properties, ReadOnly Properties with Logic :  
        // Computed Property : Not contain a value, but compute value based on other properties
        // Derived Property : Value derived from other properties

        public string DateDisplay => $"{StartDate : MMMM dd, yyyy)}";
        public string TimeRangeDisplay => $"{StartDate : hh:mm tt} - {EndDate : hh:mm tt}";
        public TimeSpan Duration => EndDate - StartDate; // Time Span represent duration between two DateTime.
        public string Status
        {
            get
            {
                if(StartDate > DateTime.Now)
                {
                    return "Upcoming";
                }
                else if (StartDate <= DateTime.Now && EndDate >= DateTime.Now)
                {
                    return "Ongoing";
                }
                else
                {
                    return "Completed";
                }
            }
        }
    }

}
