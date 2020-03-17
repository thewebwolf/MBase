using MBase.MemberService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBase.MemberService
{
    public class Service : ServiceBase, IService
    {

        public Service() : base()
        {

        }

        public string Name => "MemberService";
    }
}
