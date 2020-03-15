using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public class Request<T> : IRequest where T : IMethod
    {
        public Request()
        {

        }
        public Request(object message, MessageEnvelope envelope)
        {
            Message = message;
            Envelope = envelope;
        }

        public object Message { get; set; }
        public MessageEnvelope Envelope { get; set; }
    }
}
