using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using GPTNet.Conversations;
using Newtonsoft.Json.Linq;

namespace GPTNet.Models
{
    public class GPTApiHuggingface : GPTApiBase
    {
        [Obsolete("Please use the constructor that takes a GPTApiProperties object instead.")]
        public GPTApiHuggingface(string apiKey, string model) 
            : this(GPTApiProperties.Create<GPTApiHuggingface>(apiKey, model)) { }

        [Obsolete("Please use the constructor that takes a GPTApiProperties object instead.")]
        public GPTApiHuggingface(string apiKey, string model, HttpClient httpClient)
            : base(GPTApiProperties.Create<GPTApiHuggingface>(apiKey, model, null,httpClient)) { }

        public GPTApiHuggingface(GPTApiProperties properties) : base(properties)
        {
            ValidateParameters(properties.ApiKey, properties.Model, properties.ApiUrl);

            // Set up HttpClient
            HttpClient = properties.HttpClient ?? new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {properties.ApiKey}");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "HuggingfaceGPT");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override GPTConversationType ConversationType => GPTConversationType.UserBot;

        public override string GetJsonPayload(
            GPTConversation request,
            JsonSerializerSettings settings,
            params Tuple<string, object>[] additionalParameters) =>
        JsonConvert.SerializeObject(new { inputs = request.Data, max_new_tokens = 1000 }, settings);



        public override string GetAssistantReply(dynamic response)
        {
            if (response is JArray)
            {
                if (response.Count > 0)
                {
                    JArray responses = (JArray)response;
                    dynamic lastResponse = responses.Last;
                    return lastResponse.generated_text;
                }
                else return "Unable to access reply";
            }
            else
            {
                // response is an object
                return response.generated_text;
            }
        }

        public override GPTConversation GenerateConversation(bool resetConversationEveryMessage, decimal temperature)
        {
            return InternalGenerateConversation(resetConversationEveryMessage, temperature, 1000);
        }
    }
}
