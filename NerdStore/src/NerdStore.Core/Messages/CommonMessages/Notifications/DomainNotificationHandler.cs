using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Core.Messages.CommonMessages.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notification;

        public DomainNotificationHandler()
        {
            _notification = new List<DomainNotification>();
        }

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            _notification.Add(message);
            return Task.CompletedTask;
        }

        public virtual List<DomainNotification> ObterNotificacoes()
        {
            return _notification;
        }

        public virtual bool TemNotificacao()
        {
            return ObterNotificacoes().Any();
        }


        public void Dispose()
        {
            _notification = new List<DomainNotification>();
        }
    }
}
