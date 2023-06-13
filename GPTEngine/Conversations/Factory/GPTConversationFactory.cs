using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Conversations.Factory
{
    public class GPTConversationFactory
    {
        private readonly Dictionary<GPTConversationType, IGPTConversationBuilder> _builders;

        public GPTConversationFactory()
        {
            _builders = new Dictionary<GPTConversationType, IGPTConversationBuilder>
            {
                {GPTConversationType.OpenAI, new OpenAIGPTConversationBuilder()},
                {GPTConversationType.UserBot, new UserBotGPTConversationBuilder()}
            };
        }

        public GPTConversation Create(GPTConversationType type, bool resetHistoryPerMessage = false)
        {
            return _builders[type].Build(resetHistoryPerMessage);
        }
    }
}
