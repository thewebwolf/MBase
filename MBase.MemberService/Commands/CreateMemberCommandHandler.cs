﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MBase;
using MBase.MemberService.Models;

namespace MBase.MemberService.Commands
{
    public class CreateMemberCommandHandler : ICommand 
    {

        public Type RequestType => typeof(CreateMemberCommand);

        public Type ResponseType => typeof(Guid);

        public CreateMemberCommandHandler()
        {

        }

        public IEnumerable<IValidator> Validators { get => new IValidator[] { new AllFieldsRequiredValidation<CreateMemberCommand>() }; }
        public string Name { get => this.GetType().Name; }
        public string Description { get => "CreateMember is the command used to create a new member."; }

        public Task<IResponse> Execute(IRequest request)
        {
            return Task.Run(() =>
            {
                request.Envelope.Id = Guid.NewGuid();
                return (IResponse)new Response<CreateMemberCommandHandler>(request.Envelope.Id, request.Envelope);
            });
        }

    }
}
