using GPTNet.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Conversations.Factory
{
    public class UserBotGPTConversationBuilder : IGPTConversationBuilder
    {
        public GPTConversationType ConversationType { get; } = GPTConversationType.UserBot;
        public GPTConversation Build(bool resetHistoryPerMessage = false)
        {
            return new GPTConversation(
                new[]{
                    new Role(RoleType.User, isUserRole:true),
                    new Role(RoleType.Bot, isActiveRole:true),
                }
                , (message)=>$"{message.Content}\n "
                , resetHistoryPerMessage
            );
        }
    }
}
