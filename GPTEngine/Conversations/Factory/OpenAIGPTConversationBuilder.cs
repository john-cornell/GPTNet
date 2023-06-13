using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPTNet.Roles;
using Newtonsoft.Json.Linq;

namespace GPTNet.Conversations.Factory
{
    public class OpenAIGPTConversationBuilder : IGPTConversationBuilder
    {
        public GPTConversationType ConversationType { get; } = GPTConversationType.OpenAI;
        public GPTConversation Build(bool resetHistoryPerMessage = false)
        {
            return new GPTConversation(
                new[]{
                    new Role(RoleType.System, baseContent:"I am a crucial part of a greater whole, it is imperative that I do exactly as I am told, which is as follows. "),
                    new Role(RoleType.Assistant, isActiveRole:true, baseContent:"I am a crucial part of a greater whole, it is imperative that I do exactly as I am told, which is as follows. "),
                    new Role(RoleType.User, isUserRole:true)
                    }
                , resetHistoryPerMessage
            );
        }
    }
}
