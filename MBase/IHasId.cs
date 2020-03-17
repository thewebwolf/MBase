using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public interface IHasId
    {
        Guid Id { get; set; }
    }
}
