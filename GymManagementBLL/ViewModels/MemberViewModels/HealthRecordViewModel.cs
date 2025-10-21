﻿using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1, 300, ErrorMessage = "Height must be greater than 0")]
        public decimal Height { get; set; }
        [Range (0.1, 500, ErrorMessage = "Weight must be greater than 0")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Blood Type Is Required")]
        [StringLength(3, ErrorMessage =" Blood Type can't be longer than 3 characters.")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; } = null!; // 
    }
}
