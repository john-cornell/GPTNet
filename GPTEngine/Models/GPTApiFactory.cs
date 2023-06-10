using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Models
{
    public class GPTApiFactory
    {
        Dictionary<Type, IGPTApi> _apis = new Dictionary<Type, IGPTApi>();

        public GPTApiFactory()
        {
            
        }

        public IGPTApi GetApi(GPTApiType type, string apiKey, string model)
        {
            return type switch
            {
                GPTApiType.Huggingface => GetApi<GPTApiHuggingface>(apiKey, model),
                GPTApiType.OpenAI => GetApi<GPTOpenAI>(apiKey, model),
                _ => throw new NotImplementedException()
            };
        }


        public TGPTApi GetApi<TGPTApi>(string apiKey, string model) where TGPTApi : IGPTApi
        {
            return GetApi<TGPTApi>(apiKey, model, null);
        }

        public TGPTApi GetApi<TGPTApi>(string apiKey, string model, HttpClient httpClient) where TGPTApi : IGPTApi
        {
            if (!_apis.ContainsKey(typeof(TGPTApi)))
            {
                _apis[typeof(TGPTApi)] = (TGPTApi)Activator.CreateInstance(typeof(TGPTApi), apiKey, model, httpClient);
            }

            return (TGPTApi)_apis[typeof(TGPTApi)];
        }
    }
}
