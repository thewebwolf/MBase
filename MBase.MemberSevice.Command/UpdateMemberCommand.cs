
using System;

namespace MBase.MemberService.Commands
{
    [Serializable]
    public class UpdateMemberCommand : IHasId
    {
        public UpdateMemberCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int ExpectedVersion { get; set; }
    }
}
