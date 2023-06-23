using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Models
{
    public class GPTApiFactory
    {
        Dictionary<Type, IGPTApi> _apis = new Dictionary<Type, IGPTApi>();
        private Dictionary<string, Func<string, string, IGPTApi>> _byType;

        public GPTApiFactory()
        {
            LoadBuilderDictionary();
        }

        public IGPTApi GetApi(GPTApiType type, string apiKey, string model)
        {
            return _byType[type.Value](apiKey, model);
        }

        private void LoadBuilderDictionary()
        {
            _byType = new Dictionary<string, Func<string, string, IGPTApi>>
            {
                ["openai"] = (key, model) => GetApi<GPTAPIOpenAI>(key, model),
                ["huggingface"] = (key, model) => GetApi<GPTApiHuggingface>(key, model)
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
