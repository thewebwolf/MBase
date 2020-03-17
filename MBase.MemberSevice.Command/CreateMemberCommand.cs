

using System;

namespace MBase.MemberService.Commands
{
    [Serializable]
    public class CreateMemberCommand 
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
