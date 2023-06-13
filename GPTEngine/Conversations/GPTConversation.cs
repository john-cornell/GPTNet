using GPTNet.Events;
using GPTNet.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GPTNet.Conversations
{
    public class GPTConversation : IConversation, IDisposable
    {
        public event EventHandler<GPTMessageEventArgs>? OnMessageAdded;

        List<GPTMessage> _messages;
        public List<IRole> Roles { get; private set; }
        public IRole ActiveRole { get; private set; }
        public IRole UserRole { get; private set; }

        readonly bool _resetConversationEachMessage;

        private Func<GPTMessage, object> _dataFormatter;

        public GPTConversation(IEnumerable<IRole> roles, bool resetConversationEachMessage) : this(roles,
            m => JObject.FromObject(new { role = m.Role, content = m.Content }), resetConversationEachMessage)
        {
        }
        
        public GPTConversation(IEnumerable<IRole> roles, Func<GPTMessage, object> dataFormatter, bool resetConversationEachMessage)
        {
            if (dataFormatter == null)
            {
                throw new ArgumentNullException(nameof(dataFormatter));
            }

            _resetConversationEachMessage = resetConversationEachMessage;
            _messages = new List<GPTMessage>();
            _dataFormatter = dataFormatter;

            ValidateAndAssignRoles(roles);

            ResetMessages();
        }

        private void ValidateAndAssignRoles(IEnumerable<IRole> roles)
        {
            //validate roles not null
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            Roles = roles.ToList();

            var activeRoles = Roles.Where(r => r.IsActiveRole).ToList();
            if (activeRoles.Count != 1)
            {
                throw new ArgumentException($"There must be exactly one active role. There are {activeRoles.Count}", nameof(roles));
            }

            var userRoles = Roles.Where(r => r.IsUserRole).ToList();
            if (activeRoles.Count != 1)
            {
                throw new ArgumentException($"There must be exactly one user role. There are {userRoles.Count}", nameof(roles));
            }

            ActiveRole = activeRoles[0];
            UserRole = userRoles[0];

            Roles.ForEach(r => r.PropertyChanged += Role_PropertyChanged);
        }

        private void Role_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ResetMessages();
        }

        public void ResetMessages()
        {
            _messages.Clear();

            foreach (var role in Roles)
            {
                if (!role.IsUserRole)
                {
                    GPTMessage? message = role.GetSetupMessage();

                    if (message != null) _messages.Add(message);
                }
            }
        }

        public void AddMessage(string message)
        {
            if (_resetConversationEachMessage)
                ResetMessages();

            OnMessageAdded?.Invoke(message, new GPTMessageEventArgs(UserRole.Description, message, GPTMessageEventArgs.MessageDirection.In));
            _messages.Add(new GPTMessage(UserRole.Description, message));
        }

        public void AddReplyFromGPT(string message)
        {
            OnMessageAdded?.Invoke(message, new GPTMessageEventArgs(ActiveRole.Description, message, GPTMessageEventArgs.MessageDirection.Out));
            _messages.Add(new GPTMessage(ActiveRole.Description, message));
        }

        public object[] Data => _messages.Select(d => _dataFormatter(d)).ToArray();
        public void Dispose()
        {
            Roles?.ForEach(r => r.PropertyChanged -= Role_PropertyChanged);
        }
    }
}
