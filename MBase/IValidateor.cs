using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public interface IValidator
    {
        List<Exception> Exceptions { get; set; }

        ValidationType Type { get; }

        bool IsValid(object message);
    }
}
