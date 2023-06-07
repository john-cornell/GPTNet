using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTEngine.Roles
{
    public enum RoleType { System, Assistant, User }
    internal interface IRole
    {
        public string Content { get; }
        public RoleType RoleType { get; }

        public GPTMessage GetSetupMessage();
    }
}
