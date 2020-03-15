using System;
using System.Collections.Generic;
using System.Text;
using MBase;
using MBase.MemberService.Models;

namespace MBase.MemberService.Methods.CreateMember
{
    public class CreateMember : ICommand, IMethod
    {
        public CreateMember(IValidator[] validators)
        {
            Validators = validators;
        }

        public IValidator[] Validators { get; private set; }
        public string Name { get => this.GetType().Name; }
        public string Description { get => "CreateMember is the command used to create a new member."; }

        public IResponse Execute(IRequest request)
        {
            return new CreateMemberResponse(Guid.NewGuid(), request.Envelope);
        }
    }
}
