using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GPTNet.Roles
{
    public abstract class RoleBehaviour
    {
        public abstract string Name { get; }
        public abstract string Content { get; }

        public Role As(RoleType roleType)
        {
            return new Role(roleType, this);
        }

        public virtual bool ResetEachTime => false;

        public static RoleBehaviour Create(string content, string name = "custom", bool resetEachTime = false)
        {
            return new CustomRoleBehaviour(content, name, resetEachTime);
        }
    }

    public class CustomRoleBehaviour : RoleBehaviour
    {
        private readonly string _name;
        private readonly string _content;
        private readonly bool _resetEachTime;

        public CustomRoleBehaviour(string content, string name = "custom", bool resetEachTime = false)
        {
            _name = Regex.Replace(name, "[^a-zA-Z0-9]", "");
            _content = content;
            _resetEachTime = resetEachTime;
        }

        public override string Name => _name;
        public override string Content => _content;
        public override bool ResetEachTime => _resetEachTime;
    }
}
