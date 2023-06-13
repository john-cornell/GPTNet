using GPTNet.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPTNet.Conversations;

namespace GPTNet.Models
{
    public class GPTApiType
    {
        private GPTApiType(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static GPTApiType OpenAI => new GPTApiType("openai");
        public static GPTApiType Huggingface => new GPTApiType("huggingface");



        public static GPTApiType CustomRole(string roleName)
        {
            return new GPTApiType(roleName);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is GPTApiType))
                return false;

            return Value == ((GPTApiType)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(GPTApiType left, GPTApiType right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(GPTApiType left, GPTApiType right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

