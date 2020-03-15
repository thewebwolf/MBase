using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public class Response<T> : IResponse where T : ICommand
    {
        public Response()
        {

        }
        public Response(object message, MessageEnvelope envelope)
        {
            Message = message;
            Envelope = envelope;
        }

        public object Message { get; set; }
        public MessageEnvelope Envelope { get; set; }
    }
}
