﻿using System;
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

        public GPTChat(string apiKey, string model, decimal temperature = 0.7m) : this(apiKey, model, GPTApiType.OpenAI, temperature: temperature)
        {
        }

        public GPTChat(string apiKey, string model, GPTApiType type, string modelVersion = null, decimal temperature = 0.7m) : this(
            GPTApiProperties.Create(type, apiKey, model, null, null, temperature)
            )
        {
        }

        public GPTChat(GPTApiProperties properties)
        {
            _gpt = new GPTApiFactory().GetApi(properties);
            _conversation = _gpt.GenerateConversation(properties.ResetConversationEveryMessage, properties.Temperature);

            _conversation.OnMessageAdded += (sender, args) => OnMessage?.Invoke(this, args);

            IRole systemRole = _conversation
                .Roles
                .Where(r => r.Description == RoleType.System.Value)
                .FirstOrDefault();

            IRole activeRole = _conversation
                .Roles
                .Where(r => r.IsActiveRole)
                .FirstOrDefault();

            if (systemRole != null) systemRole.Content = "You are a friendly AI Assistant";
            if (activeRole != null) activeRole.Content = "I am a friendly AI Assistant";
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
