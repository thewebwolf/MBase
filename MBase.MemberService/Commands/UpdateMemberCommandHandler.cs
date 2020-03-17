using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MBase;
using MBase.MemberService.Commands;
using MBase.MemberService.Models;
using NServiceBus;

namespace MBase.MemberService.Command
{
    public class UpdateMemberCommandHandler : ICommand, IHandleMessages<UpdateMemberCommand>
    {

        public Type RequestType => typeof(UpdateMemberCommand);

        public Type ResponseType => typeof(int);

        public UpdateMemberCommandHandler()
        {

        }

        public IEnumerable<IValidator> Validators { get => new IValidator[] { new AllFieldsRequiredValidation<UpdateMemberCommand>(), new IdFieldValidation<UpdateMemberCommand>() }; }
        public string Name { get => this.GetType().Name; }
        public string Description { get => "UpdateMember is the command used to update a member record."; }

        public Task<IResponse> Execute(IRequest request)
        {
            return Task.Run(() =>
            {
                request.Envelope.Id = Guid.NewGuid();
                return (IResponse)new Response<UpdateMemberCommandHandler>(new Random().Next(1, int.MaxValue), request.Envelope);
            });
        }

        public Task Handle(UpdateMemberCommand message, IMessageHandlerContext context)
        {
            return Execute(new Request<UpdateMemberCommandHandler>(message, new MessageEnvelope() { Id = Guid.NewGuid() }));
        }

    }
}
