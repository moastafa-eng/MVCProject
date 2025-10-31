using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();

            return View(members);
        }

        // this is Id comes from route data in Views it comes with URL (Action Parameter Binding)
        public IActionResult MemberDetails(int Id) 
        {
            var memberDetails = _memberService.GetMemberDetails(Id);

            if(memberDetails == null)
            {
                TempData["ErrorMessage"] = "Member not found.";

                return RedirectToAction("Index");
            }

            return View(memberDetails);
        }

        public IActionResult HealthRecordDetails(int Id)
        {
            var healthRecordDetails = _memberService.GetMemberHealthRecord(Id);

            if(healthRecordDetails == null)
            {
                TempData["ErrorMessage"] = "Health record not found.";

                return RedirectToAction("Index");
            }

            return View(healthRecordDetails);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel input)
        {
            if(!ModelState.IsValid) // ModelState : Represents the state of model binding and validation it comes from ControllerBase class
            {
                ModelState.AddModelError("DataMissed", "Check Missing Data"); // adding a general error message

                return View("Create", input); // return to the Create View with the input data to show validation errors
            }

            bool result = _memberService.CreateMember(input);

            if(result)
                TempData["SuccessMessage"] = "Member created successfully.";

            else
                TempData["ErrorMessage"] = "Failed to create member.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult MemberEdit(int Id)
        {
            var member = _memberService.GetMemberToUpdate(Id);

            if(member == null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }

            return View(member);
        }

        [HttpPost]
        public IActionResult MemberEdit([FromRoute] int Id, MemberToUpdateViewModel input)
        {
            #region Error in validation
            //if (!ModelState.IsValid) // ModelState : Represents the state of model binding and validation it comes from ControllerBase class
            //{

            //    return View("MemberEdit", input); // return to the Create View with the input data to show validation errors
            //} 
            #endregion

            bool result = _memberService.UpdateMemberDetails(Id, input);

            if (result)
                TempData["SuccessMessage"] = "Member updated successfully.";

            else
                TempData["ErrorMessage"] = "Failed to update member.";

            return RedirectToAction("Index");
        }

        public IActionResult Delete([FromRoute]int Id)
        {
            if(Id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id.";
                return RedirectToAction("Index");
            }

            var member = _memberService.GetMemberDetails(Id);

            if(member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = Id;

            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            var result = _memberService.RemoveMember(id);

            if (result)
                TempData["SuccessMessage"] = "Member deleted successfully.";

            else
                TempData["ErrorMessage"] = "Failed to delete member.";

            return RedirectToAction("Index");
        }
    }
}
