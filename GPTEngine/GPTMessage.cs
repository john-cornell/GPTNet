﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTEngine
{
    public class GPTMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }

        public GPTMessage(string role, string content)
        {
            Role = role;
            Content = content;
        }
    }

}
