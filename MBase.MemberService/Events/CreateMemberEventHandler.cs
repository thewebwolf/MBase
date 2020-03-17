using System;
using System.Collections.Generic;
using System.Text;

namespace MBase.MemberService.Events
{
    public class CreateMemberEventHandler
    {
        public IEnumerable<Type> IHandle
        {
            get
            {
                return new Type[] { typeof(CreateMemberEvent) };
            }
        }

        public void Handle(IEvent newMemberEvent)
        {
            var currentEvent = (CreateMemberEvent)newMemberEvent;
            Console.WriteLine("A new Member request event was received and Handled");

        }

    }
}