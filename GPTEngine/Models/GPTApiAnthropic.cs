using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using System.Net.Http;
using GPTNet.Conversations;

namespace GPTNet.Models
{
    public class GPTApiAnthropic : GPTApiBase
    {
        private string _model;

        public GPTApiAnthropic(GPTApiProperties properties) : base(properties)
        {
            _model = properties.Model;
            ValidateParameters(properties.ApiKey, properties.Model, properties.ApiUrl, (properties.ModelVersion, "Model Version"));

            // Set up HttpClient
            HttpClient = properties.HttpClient ?? new HttpClient();

            HttpClient.DefaultRequestHeaders.Add("anthropic-version", properties.ModelVersion??"2023-06-01");
            HttpClient.DefaultRequestHeaders.Add("x-api-key", properties.ApiKey);
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override GPTConversationType ConversationType => GPTConversationType.Anthropic;

        public override string GetJsonPayload(GPTConversation request, JsonSerializerSettings settings, params Tuple<string, object>[] additionalParameters) => 
            JsonConvert.SerializeObject(
                new { 
                    model = _model, 
                    prompt = GetPrompt(request),
                    temperature = Properties.Temperature,
                    max_tokens_to_sample = 1000,
                }, settings);

        private string GetPrompt(GPTConversation request)
        {
            string prompt = string.Join(
                "",
                request.Data.OfType<string>()
                    .SkipWhile(item => !item.StartsWith("\n\nHuman:"))
                    .ToArray());
            if (prompt.Trim() == string.Empty) prompt = "\n\nHuman: ";

            return $"{prompt}\n\nAssistant:";
        }

        public override string GetAssistantReply(dynamic response) => response.completion;

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

        public override GPTConversation GenerateConversation(bool resetConversationEveryMessage, decimal temperature)
        {
            return InternalGenerateConversation(resetConversationEveryMessage, temperature, 100000);
        }
    }
}
