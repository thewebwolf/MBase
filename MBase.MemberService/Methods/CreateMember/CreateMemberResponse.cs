using System;
using System.Collections.Generic;
using System.Text;

namespace MBase.MemberService.Methods.CreateMember
{
    public class CreateMemberResponse : IResponse
    {
        public CreateMemberResponse(Guid id, IMessageEnvelope envelope)
        {
            this.Message = id;
            this.Envelope = envelope;
        }

        public IMessageEnvelope Envelope { get; set; }
        public object Message { get; set; }
    }
}
