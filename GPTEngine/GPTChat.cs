using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly Role _system;
        private readonly Role _assistant;

        private readonly Conversation _conversation;

        public GPTChat(string apiKey, string model, GPTApiType type = GPTApiType.OpenAI)
        {
            _system = new Role(RoleType.System, new CustomRoleBehaviour("This system is a simple chat interface for users to talk to the AI"));
            _assistant = new Role(RoleType.Assistant, new CustomRoleBehaviour("This assistant is a simple chat bot for users to talk to the AI. Be friendly, be funny, be helpful and don't say anything to upset. I know you'll be great :)"));

            _conversation = new Conversation(_system, _assistant, false);

            _gpt = new GPTApiFactory().GetApi(type, apiKey, model);
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
