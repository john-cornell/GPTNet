using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Conversations.Factory
{
    public interface IGPTConversationBuilder
    {
        public GPTConversationType ConversationType { get; }
        public GPTConversation Build(bool resetHistoryPerMessage);
    }
}
