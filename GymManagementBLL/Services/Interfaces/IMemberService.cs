using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    // We will Inject this Interface Into The Controller to Use;
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMemberViewModel);

        bool UpdateMemberDetails(int memberId ,MemberToUpdateViewModel updateMemberDetails);

        bool RemoveMember(int memberId);

        MemberViewModel? GetMemberDetails(int memberId); // we but ? after MemberViewModel because this function maybe return null.

        HealthRecordViewModel? GetMemberHealthRecord(int memberId); // we but ? after HealthRecordViewModel because this function maybe return null.


        MemberToUpdateViewModel? GetMemberToUpdate(int memberId);


    }
}
