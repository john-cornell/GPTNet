using GPTNet.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Conversations
{
    public interface IConversation
    {
        event EventHandler<GPTMessageEventArgs> OnMessageAdded;

        void AddMessage(string message);
        void AddReplyFromGPT(string message);
        object[] Data { get; }
    }
}
