using System;
using System.Collections.Generic;
using System.Text;

namespace MBase.MemberService.Events
{
    [Serializable]
    public class CreateMemberEvent : IEvent
    {
        public MessageEnvelope MessageEnvelope { get; set; }

        public Guid MemberId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
