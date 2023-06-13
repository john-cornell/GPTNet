using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using System.Net.Http;
using GPTNet.Conversations;

namespace GPTNet.Models
{
    public class GPTOpenAI : GPTApiBase
    {
        private string _model;

        public GPTOpenAI(string apiKey, string model) : this(apiKey, model, null) { }
        public GPTOpenAI(string apiKey, string model, HttpClient httpClient) : base(
            GPTApiType.OpenAI, "https://api.openai.com/v1/chat/completions")
        {
            _model = model;
            ValidateParameters(apiKey, model);

            // Set up HttpClient
            HttpClient = httpClient ?? new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
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
    }
}
