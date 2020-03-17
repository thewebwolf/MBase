using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MBase
{
    public interface ICommand
    {
        IEnumerable<IValidator> Validators { get; }
        string Name { get; }
        string Description { get; }
        Type RequestType { get; }
        Type ResponseType { get; }

        Task<IResponse> Execute(IRequest request);
    }
}
