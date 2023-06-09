using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;

namespace GPTNet
{
    public class GPT
    {
        private string _apiUrl;
        private string _model;
        private string _apiKey;

        HttpClient _httpClient = new HttpClient();
        public GPT(string apiKey, string model) : this(apiKey, model, null) { }
        /// <summary>
        /// Injecting httpClient for testing purposes, though feel free to use your own :)
        /// </summary>
        public GPT(string apiKey, string model, HttpClient httpClient)
        {
            _apiUrl = "https://api.openai.com/v1/chat/completions";
            _apiKey = apiKey;
            _model = model;

            // Set up HttpClient
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Architext");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<GPTResponse> Call(Conversation request)
        {
            GPTResponse response = null;

            // Serialize chat messages with the model property
            var settings = GetJsonSerializerSettings();
            string jsonPayload = JsonConvert.SerializeObject(new { model = _model, messages = request.Data, temperature = 0.1 }, settings);


            // Send the request
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await _httpClient.PostAsync(_apiUrl, content);

            // Deserialize the response
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
