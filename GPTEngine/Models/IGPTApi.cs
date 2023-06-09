﻿using GPTNet.Conversations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Models
{
    public interface IGPTApi
    {
        GPTApiType ApiType { get; }
        GPTConversationType ConversationType { get; }
        Task<GPTResponse> Call(GPTConversation request);
        JsonSerializerSettings GetJsonSerializerSettings();

        GPTConversation GenerateConversation(bool resetConversationEveryMessage, decimal temperature);
    }

}
