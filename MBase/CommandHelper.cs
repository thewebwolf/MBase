using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBase
{
    public static class CommandHelper
    {

        public static async Task<IResponse> ExecuteMethod<T>(T method, IRequest request) where T : ICommand
        {
            return await Task.Run(async () =>
             {
                 foreach (var validation in method.Validators.Where(v => v.Type.HasFlag(ValidationType.Input)))
                 {
                     if (!validation.IsValid(request.Message))
                     {
                         request.Envelope.HasErrors = true;
                         request.Envelope.Exceptions = validation.Exceptions.ToArray();
                     }
                 }

                 if (request.Envelope.HasErrors)
                 {
                     return new Response<T>(null, request.Envelope);
                 }
                 else
                 {
                     var response = await method.Execute(request);

                     foreach (var validation in method.Validators.Where(v => v.Type.HasFlag(ValidationType.Output)))
                     {
                         if (!validation.IsValid(request.Message))
                         {
                             response.Envelope.HasErrors = true;
                             request.Envelope.Exceptions = validation.Exceptions.ToArray();
                         }
                     }

                     return response;
                 }
             });

        }
    }
}
