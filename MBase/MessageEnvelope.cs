using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    [Serializable]
    public class MessageEnvelope
    {
        public MessageEnvelope()
        {

        }

        public Guid Id { get; set; }

        public MessageEnvelope(Exception[] exceptions, bool hasErrors)
        {
            Exceptions = exceptions;
            HasErrors = hasErrors;
        }

        public Exception[] Exceptions { get; set; }
        public bool HasErrors { get; set; }
    }
}
