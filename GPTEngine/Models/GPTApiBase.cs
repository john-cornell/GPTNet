using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;

namespace GPTNet.Models
{
    public abstract class GPTApiBase : IGPTApi
    {
        protected string ApiUrl { get; private set; }
        public HttpClient HttpClient { get; protected set; }

        public GPTApiBase(GPTApiType apiType, string apiUrl)
        {
            ApiUrl = apiUrl;
            ApiType = apiType;
        }

        public GPTApiType ApiType { get; }
        public abstract Task<GPTResponse> Call(Conversation request);

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
    public enum GPTApiType
    {
        Unknown = 0,
        Huggingface,
        OpenAI
    }
}
