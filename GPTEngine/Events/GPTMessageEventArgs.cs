using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Events
{
    public class GPTMessageEventArgs : EventArgs
    {
        public enum MessageDirection { In, Out }

        public string Name { get; set; }
        public string Message { get; set; }
        public MessageDirection Direction { get; set; } 

        public GPTMessageEventArgs(string name, string message, MessageDirection messageDirection)
        {
            Direction = messageDirection;
            Name = name;
            Message = message;
        }
    }
}
