using GPTNet.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Conversations
{
    public class GPTConversationType
    {
        private GPTConversationType(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static GPTConversationType OpenAI => new GPTConversationType("openai");
        public static GPTConversationType UserBot => new GPTConversationType("userbot");
        
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is GPTConversationType))
                return false;

            return Value == ((GPTConversationType)obj).Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(GPTConversationType left, GPTConversationType right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(GPTConversationType left, GPTConversationType right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
