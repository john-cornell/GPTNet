using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTEngine
{
    public class GPTResponse
    {
        public Conversation Request { get; private set; }
        public bool IsError { get; private set; }
        public string Error { get; private set; }
        public string Response { get; private set; } 
        private GPTResponse(Conversation request, string response, string error)
        {
            Request = request;
            IsError = !string.IsNullOrWhiteSpace(error);
            Error = error;
            Response = response;
        }

        public static GPTResponse Success(Conversation request, string response) => new GPTResponse(request, response, string.Empty);
        public static GPTResponse Failure(Conversation request, string error) => new GPTResponse(request, string.Empty, error);
    }
}
