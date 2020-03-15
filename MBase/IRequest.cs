using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public interface IRequest
    {
        object Message { get; set; }
        MessageEnvelope Envelope { get; set; }
    }
}
