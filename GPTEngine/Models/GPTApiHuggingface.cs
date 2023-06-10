using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Configuration;
using System.Text;
using GPTNet;

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

        public override async Task<GPTResponse> Call(Conversation request)
        {
            GPTResponse response = null;

            // Serialize chat messages with the model property
            var settings = GetJsonSerializerSettings();
            string jsonPayload = JsonConvert.SerializeObject(new { inputs = $"Human: {request.Data} Bot:", max_new_tokens = 25 }, settings);

            // Send the request
            HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponse = await HttpClient.PostAsync(ApiUrl, content);

            // Deserialize the response
            if (httpResponse.IsSuccessStatusCode)
            {
                string responseJson = await httpResponse.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseJson);
                string assistantReply = responseObject[0].generated_text;

                request.AddReplyFromGPT(assistantReply);

                response = GPTResponse.Success(request, assistantReply);
            }
            else
            {
                response = GPTResponse.Failure(request, httpResponse.StatusCode.ToString());
            }

            return response;
        }
    }
}
