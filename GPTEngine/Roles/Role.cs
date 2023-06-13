using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GPTNet.Roles
{
    public class Role : IRole
    {
        public Role(RoleType roleType, bool isActiveRole = false, bool isUserRole = false)
        {
            RoleType = roleType;
            IsActiveRole = isActiveRole;
            IsUserRole = isUserRole;

            //User doesn't need to be told to be a good bot
            BaseContent = IsUserRole
                ? ""
                : "I am a crucial part of a greater whole, it is imperative that I do exactly as I am told, which is: ";
        }

        public bool IsActiveRole { get; }
        public bool IsUserRole { get; }

        public string BaseContent { get; set; } 
     
        private string _roleContent;

        public string Content
        {
            get => $"{BaseContent} {_roleContent ?? string.Empty}";
            set
            {
                _roleContent = value; 

                OnPropertyChanged(nameof(Content));
            }
        }

        public RoleType RoleType { get; }
        public string Description => RoleType.Value;

        public GPTMessage GetSetupMessage() => new GPTMessage(RoleType.ToString().ToLowerInvariant(), Content);

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    }
}
