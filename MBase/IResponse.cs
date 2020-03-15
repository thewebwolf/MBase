using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public interface IResponse
    {
        object Message { get; set; }
        MessageEnvelope Envelope { get; set; }
    }
}
