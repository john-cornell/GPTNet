using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Events
{
    public class GPTErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
        public GPTErrorEventArgs(string error)
        {
            ErrorMessage = error;
        }
    }
}
