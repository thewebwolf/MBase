using MBase.MemberService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBase.MemberService.Models
{
    public class AllFieldsRequiredValidation<T> : IValidator
    {
        public ValidationType Type => ValidationType.Input;

        public List<Exception> Exceptions { get; set; } = new List<Exception>();

        public bool IsValid(object message)
        {
            var castMessage = (T)message;
           
            var x = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Any(p => p.GetValue(castMessage) != null);
            if (x)
            {
                return true;
            }
            else
            {
                Exceptions.Add(new Exception("All Fields are Mandatory."));

                return false;
            }
        }
    }
}
