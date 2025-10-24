using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.interfaces;

namespace GymManagementBLL.Services.Classes
{
    // Implementation of the IMemberService interface

    // MemberService Takes Data from Member Repository and process it to return MemberViewModel 
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Getting Reference from Member Repository


        // Injecting Member Repository via constructor
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateMember(CreateMemberViewModel model)
        {
            try
            {
                if (IsEmailExists(model.Email) || IsPhoneExists(model.Phone))
                {
                    return false; // Email or Phone already exists
                }

                var member = new Member
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = new Address
                    {
                        BuildingNumber = model.BuildingNumber,
                        City = model.City,
                        Street = model.Street
                    },
                    HealthRecord = new HealthRecord
                    {
                        Height = model.HealthRecordViewModel.Height,
                        Weight = model.HealthRecordViewModel.Weight,
                        BloodType = model.HealthRecordViewModel.BloodType,
                        Note = model.HealthRecordViewModel.Note
                    }
                };

                _unitOfWork.GetRepository<Member>().Add(member);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveMember(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
                return false;

            var activeBooking = _unitOfWork.GetRepository<Booking>().GetAll(x => x.MemberId == memberId 
            && x.Session.StartDate > DateTime.Now);

            if (activeBooking.Any())
                return false;

            var memberShips = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == memberId);

            try
            {
                if (memberShips.Any())
                {
                    foreach(var memberShip in memberShips)
                    {
                        _unitOfWork.GetRepository<MemberShip>().Delete(memberShip);
                    }
                }

                _unitOfWork.GetRepository<Member>().Delete(member);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            // Get  IEnumerable<Member> from Member Repository
            var members = _unitOfWork.GetRepository<Member>().GetAll() ?? []; // [] means empty list if null

            if (members is null || !members.Any()) // [member is null] is unnecessary due to null-coalescing above
            {
                return []; // Return an empty list if no members found
            }

            // Mapping Member entities to MemberViewModel
            // IEnumerable<MemberViewModel>
            var memberViewModels = members.Select(x => new MemberViewModel // Mapping between entity and ViewMdel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                DateOfBirth = x.DateOfBirth.ToShortDateString(),
                Photo = x.Photo,
                Gender = x.Gender.ToString(),
            });

            // return IEnumerable<MemberViewModel>
            return memberViewModels;
        }

        // This method retrieves detailed information about a specific member using their ID.
        // It first fetches the member entity from the repository and maps it into a MemberViewModel.
        // Then, it checks if the member has an active membership (Status = "Active").
        // If an active membership exists, it fetches the related plan details (name, start date, and end date)
        // and adds them to the view model.
        // Returns null if the member does not exist.
        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
            {
                return null;
            }

            var memberViewModel = new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                Address = FormatAddress(member.Address),

            };

            var activeMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(x =>
            x.MemberId == memberId
            && x.Status == "Active")
                .FirstOrDefault();

            if (activeMemberShip is not null)
            {
                var activePlans = _unitOfWork.GetRepository<Plan>().GetById(activeMemberShip.PlanId);

                memberViewModel.PlanName = activePlans?.Name;
                memberViewModel.MemberShipStartDate = activeMemberShip.CreatedAt.ToLongDateString();
                memberViewModel.MemberShipEndDate = activeMemberShip.EndDate.ToLongDateString();
            }

            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            // because HealthRecord and Member in the same table relation ship (One to one)
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);

            if (memberHealthRecord is null)
            {
                return null;
            }

            var HealthRecordViewModel = new HealthRecordViewModel
            {
                Weight = memberHealthRecord.Weight,
                Height = memberHealthRecord.Height,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note

            };

            return HealthRecordViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
            {
                return null;
            }

            var memberToUpdateViewModel = new MemberToUpdateViewModel
            {
               
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,

            };

            return memberToUpdateViewModel;
        }

        public bool UpdateMemberDetails(int memberId ,MemberToUpdateViewModel model)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member is null)
            {
                return false;
            }

            if (IsEmailExists(model.Email) || IsPhoneExists(model.Phone))
            {
                return false; // Email or Phone already exists
            }

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.CreatedAt = DateTime.Now;


            _unitOfWork.GetRepository<Member>().Update(member);
            _unitOfWork.SaveChanges();
            return true;
        }


        #region Helper Methods

        private string FormatAddress (Address address)
        {
            if(address is null)
            {
                return "N/A";
            }
            return $"{address.BuildingNumber}, {address.Street}, {address.City}";
        }

        private bool IsEmailExists (string email)
        {
            var ExistingEmail = _unitOfWork.GetRepository<Member>().GetAll(x=> x.Email == email);
            return ExistingEmail is not null && ExistingEmail.Any();
        }

        private bool IsPhoneExists(string phone)
        {
            var ExistingPhone = _unitOfWork.GetRepository<Member>().GetAll(x => x.Phone == phone);
            return ExistingPhone is not null && ExistingPhone.Any();
        }

        #endregion
    }
}
