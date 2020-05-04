//using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNet.SignalR.Hubs;
using PowerAssinger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static PowerAssinger.Services.PowerRequestSolver;

namespace PowerAssinger.HubConfig
{
    [HubName("assingmentsHub")]
    public class AssingmentsHub : Hub { }
}
