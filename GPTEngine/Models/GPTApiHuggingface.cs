using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using GPTNet.Conversations;

namespace GPTNet.Models
{
    public class GPTApiHuggingface : GPTApiBase
    {
        public GPTApiHuggingface(string apiKey, string model) : this(apiKey, model, null) { }
        public GPTApiHuggingface(string apiKey, string model, HttpClient httpClient) : base(GPTApiType.Huggingface, $"https://api-inference.huggingface.co/models/{model}")
        {
            // Set up HttpClient
            HttpClient = httpClient ?? new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "HuggingfaceGPT");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public override GPTConversationType ConversationType => GPTConversationType.UserBot;

        public override string GetJsonPayload(
            GPTConversation request, 
            JsonSerializerSettings settings, 
            params Tuple<string, object>[] additionalParameters) => 
        JsonConvert.SerializeObject(new { inputs = request.Data, max_new_tokens = 1000 }, settings);


        public override string GetAssistantReply(dynamic response) => response[0].generated_text;

    }
}
