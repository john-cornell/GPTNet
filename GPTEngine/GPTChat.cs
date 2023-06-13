using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GPTNet.Conversations;
using GPTNet.Conversations.Factory;
using GPTNet.Events;
using GPTNet.Models;
using GPTNet.Roles;

namespace GPTNet
{
    public class GPTChat
    {
        public event EventHandler<GPTMessageEventArgs> OnMessage;
        public event EventHandler<GPTErrorEventArgs> OnError;

        private readonly IGPTApi _gpt;
        
        private readonly GPTConversation _conversation;

        public GPTChat(string apiKey, string model) : this(apiKey, model, GPTApiType.OpenAI)
        {
        }

        public GPTChat(string apiKey, string model, GPTApiType type)
        {
            _gpt = new GPTApiFactory().GetApi(type, apiKey, model);
            _conversation = new GPTConversationFactory().Create(_gpt.ConversationType, true);
            //_conversation.ActiveRole.Content =
            //    "This assistant is a simple chat bot for users to talk to the AI. Be friendly, be funny, be helpful and don't say anything to upset. I know you'll be great :)";

            //_conversation.ActiveRole.Content =
            //    "Please continue this conversation as a simple helpful chat bot.";

            //Simply for OpenAI Chat system, which carries a lot of weight
            IRole systemRole = _conversation
                .Roles
                .Where(r=>r.Description == RoleType.System.Value)
                .FirstOrDefault();

            if (systemRole != null) systemRole.Content = "This system is a simple chat interface for users to talk to the AI";
        }

        public async Task<GPTResponse> Chat(string message)
        {
            _conversation.AddMessage(message);

            GPTResponse response = await _gpt.Call(_conversation);

            if (response.IsError)
            {
                OnError?.Invoke(this, new GPTErrorEventArgs(response.Error));
                return response;
            }

            return response;
        }
    }
}
