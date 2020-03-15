using System;
using System.Collections.Generic;
using System.Text;

namespace MBase
{
    public interface IService
    {
        string Name { get; }
        IEnumerable<ICommand> Commands { get; }
    }
}
