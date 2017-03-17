using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace HiddenSound.API.Hubs
{
    public class AuthorizationHub : Hub<IClient>
    {
        public void BroadcastAuthroization()
        {
            
        }
    }
}
