using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using System.Net.Http;
using GPTNet.Conversations;

namespace GPTNet.Models
{
    public class GPTApiOpenAI : GPTApiBase
    {
        private string _model;
        [Obsolete("Please use the constructor that takes a GPTApiProperties object instead.")]
        public GPTApiOpenAI(string apiKey, string model) : this(
            new GPTApiProperties
            {
                ApiType = GPTApiType.OpenAI,
                ApiUrl = $"https://api.openai.com/v1/chat/completions",
                ApiKey = apiKey,
                Model = model
            })
        { }

        [Obsolete("Please use the constructor that takes a GPTApiProperties object instead.")]
        public GPTApiOpenAI(string apiKey, string model, HttpClient httpClient) : base(
            new GPTApiProperties
            {
                ApiType = GPTApiType.OpenAI,
                ApiUrl = $"https://api.openai.com/v1/chat/completions",
                ApiKey = apiKey,
                Model = model,
                HttpClient = httpClient
            })
        {
        }
        public GPTApiOpenAI(GPTApiProperties properties) : base(properties)
        {
            _model = properties.Model;
            ValidateParameters(properties.ApiKey, properties.Model, properties.ApiUrl);

            // Set up HttpClient
            HttpClient = properties.HttpClient ?? new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {properties.ApiKey}");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "Architext");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override GPTConversationType ConversationType => GPTConversationType.OpenAI;

        public override string GetJsonPayload(GPTConversation request, JsonSerializerSettings settings, params Tuple<string, object>[] additionalParameters) => JsonConvert.SerializeObject(new { model = _model, messages = request.Data, temperature = 0.1 }, settings);

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
            return InternalGenerateConversation(resetConversationEveryMessage, temperature, 4000);
        }
    }
}
