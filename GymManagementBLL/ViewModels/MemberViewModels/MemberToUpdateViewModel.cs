using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {

        public string Name { get; set; } = null!;
        public string? Photo { get; set; }


        [Required(ErrorMessage = ("Email Is Required"))]
        [EmailAddress(ErrorMessage = "Invalid Email Address Format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Please enter a valid Egyptian phone number starting with 010, 011, 012, or 015 and containing 11 digits in total.")]
        [Phone(ErrorMessage = "Invalid Phone Number Format")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than 0.")]
        public int BuildingNumber { get; set; }
        [Required(ErrorMessage = "Street Is Required")]
        [RegularExpression(@"^\p{L}+(?: \p{L}+)*$", ErrorMessage = "City name can only contain letters and single spaces — no numbers or special characters.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters long.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Street Is Required")]
        [RegularExpression(@"^[\p{L}\d\s\-/]+$", ErrorMessage = "Street name can contain letters, numbers, spaces, dashes, or slashes only.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters long.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Health Record Is Required")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
}
