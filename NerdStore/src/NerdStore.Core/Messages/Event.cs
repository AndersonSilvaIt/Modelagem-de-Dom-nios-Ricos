using MediatR;
using System;

namespace NerdStore.Core.Messages
{
    public abstract class Event : Message, INotification
    {
        public DateTime TimesTamp { get; private set; }

        public Event()
        {
            TimesTamp = DateTime.Now;
        }
    }
}