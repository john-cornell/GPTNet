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
        private Dictionary<string, Func<GPTApiProperties, IGPTApi>> _byType;

        public GPTApiFactory()
        {
            LoadBuilderDictionary();
        }

        public IGPTApi GetApi(GPTApiProperties properties)
        {
            if (!_byType.ContainsKey(properties.ApiType.Value))
            {
                throw new ArgumentException($"Unknown GPTApiType: {properties.ApiType}");
            }

            return _byType[properties.ApiType.Value](properties);
        }

        //Backwards compatibility, new models may not be added here
        //OpenAI, Huggingface, and Anthropic are the only supported models currently from this method
        [Obsolete("Use GetApi(GPTApiProperties) instead")]
        public IGPTApi GetApi(GPTApiType type, string apiKey, string model)
        {
            if (type == GPTApiType.OpenAI)
            {
                return GetApi<GPTApiOpenAI>(apiKey, model);
            }
            else if (type == GPTApiType.Huggingface)
            {
                return GetApi<GPTApiHuggingface>(apiKey, model);
            }
            else if (type == GPTApiType.Anthropic)
            {
                return GetApi<GPTApiAnthropic>(apiKey, model);
            }
            else
            {
                throw new ArgumentException($"Unknown GPTApiType: {type}");
            }
        }

        private void LoadBuilderDictionary()
        {
            _byType = new Dictionary<string, Func<GPTApiProperties, IGPTApi>>
            {
                //The GPTApiType "enum" is in fact a string in a custom type,
                //so they can be created programatically, hence not const, so 
                //can't be used as a dictionary key. Bummer dude.
                ["openai"] = (properties) => GetApi<GPTApiOpenAI>(properties),
                ["huggingface"] = (properties) => GetApi<GPTApiHuggingface>(properties),
                ["anthropic"] = (properties) => GetApi<GPTApiAnthropic>(properties)
            };
        }


        public TGPTApi GetApi<TGPTApi>(string apiKey, string model, HttpClient httpClient = null, string modelVersion = null) where TGPTApi : IGPTApi
        {
            return GetApi<TGPTApi>(
                GPTApiProperties.Create<TGPTApi>(apiKey, model, modelVersion, httpClient));
        }

        public TGPTApi GetApi<TGPTApi>(GPTApiProperties properties) where TGPTApi : IGPTApi
        {
            if (!_apis.ContainsKey(typeof(TGPTApi)))
            {
                _apis[typeof(TGPTApi)] = 
                    (TGPTApi)Activator.CreateInstance(
                        typeof(TGPTApi), 
                        properties);
            }

            return (TGPTApi)_apis[typeof(TGPTApi)];
        }
    }
}
