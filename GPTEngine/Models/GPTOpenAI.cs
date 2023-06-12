using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using System.Net.Http;

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

        public override async Task<GPTResponse> Call(Conversation request)
        {
            GPTResponse response = null;

            // Serialize chat messages with the model property
            var settings = GetJsonSerializerSettings();
            string jsonPayload = JsonConvert.SerializeObject(new { model = _model, messages = request.Data, temperature = 0.1 }, settings);


            // Send the request
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await HttpClient.PostAsync(ApiUrl, content);

            // Deserialize the response - I could probably push more of this down to base but want a better idea from a few more models what the differences are
            if (httpResponse.IsSuccessStatusCode)
            {
                string responseJson = await httpResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseJson);
                string assistantReply = responseObject.choices[0].message.content;

                request.AddReplyFromGPT(assistantReply);

                response = GPTResponse.Success(request, assistantReply);
            }
            else
            {
                response = GPTResponse.Failure(request, httpResponse.StatusCode.ToString());
            }

            return response;
        }
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
