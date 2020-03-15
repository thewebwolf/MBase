using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public interface IEvent
    {
        MessageEnvelope MessageEnvelope { get; set; }
    }
}
