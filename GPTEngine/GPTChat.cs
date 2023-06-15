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

        public GPTChat(string apiKey, string model, decimal temperature = 0.7m) : this(apiKey, model, GPTApiType.OpenAI)
        {
        }

        public GPTChat(string apiKey, string model, GPTApiType type, decimal temperature = 0.7m)
        {
            _gpt = new GPTApiFactory().GetApi(type, apiKey, model);
            _conversation = _gpt.GenerateConversation(false, temperature);
            
            IRole systemRole = _conversation
                .Roles
                .Where(r=>r.Description == RoleType.System.Value)
                .FirstOrDefault();

            IRole activeRole = _conversation                .Roles
                .Where(r => r.IsActiveRole)
                .FirstOrDefault();

            if (systemRole != null) systemRole.Content = "You are a friendly AI Assistant";
            if (activeRole != null) activeRole.Content = "You are a friendly AI Assistant";
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
