using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Transports;

namespace VXI_GAMS_US.Hub
{
    public class HubUsers
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }

    public class GlobalHub : Microsoft.AspNet.SignalR.Hub
    {
        public static ConcurrentBag<HubUsers> Users = new ConcurrentBag<HubUsers>();
        public Task ReloadPage()
        {
            return Clients.Others.reloadPages();
        }

        public Task GetVersion()
        {
            return Clients.Caller.getVersion(ConfigurationManager.AppSettings["IMPORT_VERSION"]);
        }

        public Task GetCount()
        {
            var heartBeat = GlobalHost.DependencyResolver.Resolve<ITransportHeartbeat>();
            var connectionIds = heartBeat.GetConnections().Where(x => x.IsAlive).Select(x=>x.ConnectionId).ToArray();
            var activeUsers = (from connectionId in connectionIds select Users.FirstOrDefault(x => x.ConnectionId == connectionId) into usr where usr != null select usr.Username).ToList();
            return Clients.Caller.setCount(activeUsers);
        }

        #region Overrides of HubBase

        public override Task OnConnected()
        {
            Users.Add(new HubUsers
            {
                ConnectionId = Context.ConnectionId,
                Username = Context.User?.Identity?.Name
            });
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            if (string.IsNullOrEmpty(Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId)?.ConnectionId))
                Users.Add(new HubUsers
                {
                    ConnectionId = Context.ConnectionId,
                    Username = Context.User?.Identity?.Name
                });
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var user = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (!string.IsNullOrEmpty(user?.ConnectionId))
                Users.TryTake(out user);
            return base.OnDisconnected(stopCalled);
        }

        #endregion
    }
}