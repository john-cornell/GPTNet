using System.Collections.ObjectModel;
using GPTNet.Events;
using GPTNet.Roles;

namespace GPTNet
{
    public class Conversation
    {
        public event EventHandler<GPTMessageEventArgs>? OnMessageAdded;

        List<GPTMessage> _messages;
        bool _resetConversationEachMessage;
        public Role System { get; private set; }
        public Role Assistant { get; private set; }

        public Conversation(RoleBehaviour role) : this(role.As(RoleType.System), role.As(RoleType.Assistant), role.ResetEachTime)
        {

        }

        public Conversation(Role system, Role assistant, bool resetConversationEachMessage)
        {
            _resetConversationEachMessage = resetConversationEachMessage;

            System = system;
            Assistant = assistant;

            if (system.RoleType != RoleType.System)
            {
                throw new ArgumentException("The first role must be of type System.", nameof(system));
            }

            if (assistant.RoleType != RoleType.Assistant)
            {
                throw new ArgumentException("The second role must be of type Assistant.", nameof(assistant));
            }

            ResetMessages();
        }

        private void ResetMessages()
        {
            _messages = new List<GPTMessage>() { System.GetSetupMessage() };
            if (_messages[0].Content != Assistant.GetSetupMessage().Content) _messages.Add(Assistant.GetSetupMessage());
        }

        public void AddMessage(string message)
        {
            if (_resetConversationEachMessage)
                ResetMessages();

            OnMessageAdded?.Invoke(message, new GPTMessageEventArgs(System.Name, message, GPTMessageEventArgs.MessageDirection.In));
            _messages.Add(new GPTMessage("user", message));
        }

        public void AddReplyFromGPT(string message)
        {
            OnMessageAdded?.Invoke(message, new GPTMessageEventArgs(System.Name, message, GPTMessageEventArgs.MessageDirection.Out));
            _messages.Add(new GPTMessage("assistant", message));
        }

        public GPTMessage[] Data => _messages.ToArray();
    }

}
