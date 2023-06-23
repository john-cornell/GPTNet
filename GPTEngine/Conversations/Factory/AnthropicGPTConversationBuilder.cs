using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPTNet.Roles;

namespace GPTNet.Conversations.Factory
{
    public class AnthropicGPTConversationBuilder : IGPTConversationBuilder
    {
        public GPTConversationType ConversationType => GPTConversationType.Anthropic;
        public GPTConversation Build(bool resetHistoryPerMessage)
        {
            return new GPTConversation(new[]
                {
                    new Role(RoleType.Assistant, isActiveRole: true),
                    new Role(RoleType.Human, isUserRole: true)
                },
                (message) => $"\n\n{UppercaseInitialLetter(message.Role)}: {UppercaseInitialLetter(message.Content)} "
                , resetHistoryPerMessage);
        }

        private string UppercaseInitialLetter(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return string.Empty;
            return char.ToUpperInvariant(roleName[0]) + roleName.Substring(1);
        }
    }
}
