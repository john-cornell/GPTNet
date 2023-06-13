using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Roles
{
    public class RoleType
    {
        private RoleType(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static RoleType System => new RoleType("system");
        public static RoleType Assistant => new RoleType("assistant");
        public static RoleType User => new RoleType("user");
        public static RoleType Bot => new RoleType("bot");
        public static RoleType CustomRole(string roleName)
        {
            return new RoleType(roleName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is RoleType))
                return false;

            return Value == ((RoleType)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(RoleType left, RoleType right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(RoleType left, RoleType right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
