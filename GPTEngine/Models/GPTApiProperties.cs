using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Models
{
    public class GPTApiProperties
    {
        //Required
        public GPTApiType ApiType { get; set; }
        public String ApiKey { get; set; }
        public string Model { get; set; }
        public string ApiUrl {get; set; }
        //Optional, largely for unit tests
        public HttpClient HttpClient { get; set; }


        //Optional
        public string ModelVersion { get; set; }
        public decimal Temperature { get; set; } = 0.7m;
        public bool ResetConversationEveryMessage { get; set; } = false;

        //Static
        static Dictionary<Type, Func<string, string, HttpClient, GPTApiProperties>> _propertyFactoriesByType;
        private static void LoadDefaults()
        {
            _propertyFactoriesByType = new Dictionary<Type, Func<string, string, HttpClient, GPTApiProperties>>();

            _propertyFactoriesByType[typeof(GPTApiOpenAI)] = (apiKey, model,httpClient ) => 
            {
                return Create(GPTApiType.OpenAI, apiKey, model, null, httpClient);
            };

            _propertyFactoriesByType[typeof(GPTApiHuggingface)] = (apiKey, model, httpClient) =>
            {
                return Create(GPTApiType.Huggingface, apiKey, model,  null, httpClient);
            };

            _propertyFactoriesByType[typeof(GPTApiAnthropic)] = (apiKey, model, httpClient) =>
            {
                return Create(GPTApiType.Anthropic, apiKey, model, null, httpClient);
            };
        }

        public static GPTApiProperties Create(GPTApiType apiType, string apiKey, string model, string modelVersion = null, HttpClient httpClient = null, decimal temperature = 0.7m)
        {
            GPTApiProperties properties = new GPTApiProperties();

            properties.ApiType = apiType;
            properties.ApiKey = apiKey;
            properties.Model = model;
            properties.ModelVersion = modelVersion;
            properties.ApiUrl = GetApiUrl(apiType, model);
            properties.HttpClient = httpClient;
            properties.Temperature = temperature;

            return properties;
        }

        public static GPTApiProperties Create<TGPTApi>(
            string apiKey, 
            string model,
            string modelVersion = null,
            HttpClient httpClient = null,
            decimal temperature = 0.7m
            ) where TGPTApi : IGPTApi
        {
            if (_propertyFactoriesByType == null)
            {
                LoadDefaults();
            }

            if (!_propertyFactoriesByType.ContainsKey(typeof(TGPTApi)))
            {
                throw new ArgumentException($"Unknown GPTApiType: {typeof(TGPTApi)}");
            }

            var properties = _propertyFactoriesByType[typeof(TGPTApi)](apiKey, model, httpClient);
            properties.ModelVersion = modelVersion;
            properties.Temperature = temperature;

            return properties;
        }

        //Not the greatest implementation, but it works.Lets hope future models don't require more than just the model name
        //or I may have to split out another object
        //which was the reason I needed GPTApiProperties in the first place (for Anthropic modelVersion)
        public static string GetApiUrl(GPTApiType apiType, string model)
        {
            switch (apiType.Value)
            {
                case "openai":
                    return "https://api.openai.com/v1/chat/completions";
                case "huggingface":
                    return $"https://api-inference.huggingface.co/models/{model}";
                case "anthropic":
                    return "https://api.anthropic.com/v1/complete";
                default:
                    throw new ArgumentException($"Unknown GPTApiType: {apiType}");
            }
        }
    }
}
