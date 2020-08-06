using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ControlCalidadAireUtec.Hubs
{
    [HubName("estacionesHub")]
    public class EstacionesHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext Icontext = GlobalHost.ConnectionManager.GetHubContext<EstacionesHub>();
            Icontext.Clients.All.refreshEstacionesData();
        }
    }
}