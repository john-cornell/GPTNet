using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Roles
{
    public interface IRole : INotifyPropertyChanged
    {
        /// <summary>
        /// Is primary communication role, i.e. assistant as opposed to system, as system isn't the role that participates in the communication.
        /// </summary>
        public bool IsActiveRole { get; }
        /// <summary>
        /// Is the role that the user is assigned to, i.e. user 
        /// </summary>
        public bool IsUserRole { get; }
        public string Content { get; set; }
        public RoleType RoleType { get; }
        public string Description { get; }
        public GPTMessage? GetSetupMessage();
    }
}
