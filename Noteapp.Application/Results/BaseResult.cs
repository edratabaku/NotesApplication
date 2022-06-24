using Noteapp.Application.Results.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteapp.Application.Results
{
    public abstract class BaseResult
    {
        private IList<Message>? _messages;

        /// <summary>
        /// Determines whether the result is successful.
        /// </summary>
        public bool Success => Messages.All(m => m.Level != MessageLevel.Error);

        public string Description { get; set; } = "";

        /// <summary>
        /// List of messages on this result.
        /// </summary>
        public IList<Message> Messages
        {
            get => _messages ??= new List<Message>();
            set => _messages = value;
        }
    }
}
