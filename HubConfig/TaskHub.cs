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

namespace PowerAssinger.HubConfig
{
    [HubName("PowerHub")]
    public class PowerHub : Hub
    {
        //private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(1000);
        //private static int currentId = 0;
        //private Timer _timer;

        ////public List<Payload> GetTasks()
        ////{
        ////    return new TaskService().GetAllTasks();
        ////}

        //public void UpdateTasks()
        //{
        //    _timer = new Timer(UpdateTaskStatus, null, _updateInterval, _updateInterval);
        //}

        //private void UpdateTaskStatus(Object state)
        //{
        //    //Clients.All.updateTaskStatus(task);
        //}
    }
}
