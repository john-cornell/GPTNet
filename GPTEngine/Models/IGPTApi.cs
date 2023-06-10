using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Models
{
    public interface IGPTApi
    {
        GPTApiType ApiType { get; }
        Task<GPTResponse> Call(Conversation request);
        JsonSerializerSettings GetJsonSerializerSettings();
    }

}
