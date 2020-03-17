using MBase.MemberService.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBase.MemberService.Models
{
    public class IdFieldValidation<T> : IValidator where T : IHasId
    {
        public ValidationType Type => ValidationType.Input;

        public List<Exception> Exceptions { get; set; } = new List<Exception>();

        public bool IsValid(object message)
        {
            var castMessage = (T)message;

            if (castMessage.Id != null && castMessage.Id!= Guid.Empty)
            {
                return true;
            }
            else
            {
                Exceptions.Add(new Exception("Member Id is required."));

                return false;
            }
        }
    }
}
