using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using GPTNet.Conversations;
using GPTNet.Conversations.Factory;

namespace GPTNet.Models
{
    public abstract class GPTApiBase : IGPTApi
    {
        protected string ApiUrl { get; private set; }
        public HttpClient? HttpClient { get; protected set; }
        public GPTApiProperties Properties { get; private set; }

        protected GPTApiBase(GPTApiProperties properties)
        {
            Properties = properties;

            ApiUrl = properties.ApiUrl;
            ApiType = properties.ApiType;
        }

        protected void ValidateParameters(string apiKey, string model, string apiUrl, params (object, string)[] customParameters)
        {
            apiUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));
            apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            model = model ?? throw new ArgumentNullException(nameof(model));

            foreach (var parameterAndName in customParameters)
            {
                if (parameterAndName.Item1 == null)
                    throw new ArgumentNullException(parameterAndName.Item2);
            }
        }

        public GPTApiType ApiType { get; }
        public abstract GPTConversationType ConversationType { get; }

        public async Task<GPTResponse> Call(GPTConversation request)
        {
            // Serialize chat messages with the model property
            var settings = GetJsonSerializerSettings();

            return await SendToGPT(request, GetJsonPayload(request, settings));
        }

        public abstract string GetJsonPayload(GPTConversation request, JsonSerializerSettings settings,
            params Tuple<string, object>[] additionalParameters);

        protected async Task<GPTResponse> SendToGPT(GPTConversation request, string jsonPayload)
        {
            // Send the request
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await HttpClient.PostAsync(ApiUrl, content);

            GPTResponse response;
            if (httpResponse.IsSuccessStatusCode)
            {
                string responseJson = await httpResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseJson);
                string assistantReply = GetAssistantReply(responseObject);

                request.AddReplyFromGPT(assistantReply);

                response = GPTResponse.Success(request, assistantReply);
            }
            else
            {
                if (!httpResponse.IsSuccessStatusCode)
                {
                    string errorContent = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {httpResponse.StatusCode}. Content: {errorContent}");
                }

                response = GPTResponse.Failure(request, httpResponse.StatusCode.ToString());
            }

            return response;
        }

        public virtual string GetAssistantReply(dynamic response) => response.choices[0].message.content;

        public JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        public abstract GPTConversation GenerateConversation(bool resetConversationEveryMessage,  decimal temperature);

        protected GPTConversation InternalGenerateConversation(bool resetConversationEveryMessage, decimal temperature, int maxTokens)
        {
            GPTConversation conversation = new GPTConversationFactory().Create(ConversationType, resetConversationEveryMessage);

            conversation.Temperature = temperature;
            conversation.MaxTokens = maxTokens;

            return conversation;
        }
    }
}
