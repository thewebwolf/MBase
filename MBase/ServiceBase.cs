using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBase
{
    public abstract class ServiceBase
    {
        public ServiceBase()
        {
            this.Commands = GetCommands();
        }

        private IEnumerable<ICommand> GetCommands()
        {
            foreach (var type in this.GetType().Assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t) && t != typeof(ICommand)))
            {
                yield return (ICommand)Activator.CreateInstance(type);
            }
        }

        public IEnumerable<ICommand> Commands { get; }

        
    }
}
