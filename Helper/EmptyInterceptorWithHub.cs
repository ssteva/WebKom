using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NHibernate;

namespace webkom.Helper
{
    public class EmptyInterceptorWithHub<THub> : EmptyInterceptor where THub : IHub
    {
        Lazy<IHubContext> _hub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        protected IHubContext Hub
        {
            get
            {
                return _hub.Value;
            }
        }
    }

}