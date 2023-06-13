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
                    new Role(RoleType.System),
                    new Role(RoleType.Assistant, isActiveRole:true),
                    new Role(RoleType.User, isUserRole:true)
                    }
                , resetHistoryPerMessage
            );
        }

        //public string DataFormatter(GPTMessage message)
        //{
        //    return $""
        //}
    }
}
