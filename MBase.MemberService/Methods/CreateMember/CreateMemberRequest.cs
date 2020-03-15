using MBase.MemberService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBase.MemberService.Methods.CreateMember
{
    public class CreateMemberRequest : IRequest
    {
        public CreateMemberRequest(object message, IMessageEnvelope envelope)
        {
            Message = message;
            Envelope = envelope;
        }

        public object Message { get; set; }
        public IMessageEnvelope Envelope { get; set; }
    }
}
