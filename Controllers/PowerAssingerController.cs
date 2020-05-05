
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PowerAssinger.Services;
using PowerAssinger.Model;
using System.Collections.Generic;
using static PowerAssinger.Services.PowerRequestSolver;
using Microsoft.AspNetCore.SignalR;
using PowerAssinger.HubConfig;
using Newtonsoft.Json;
using System;

namespace PowerAssinger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerAssingerController : ControllerBase
    {
        private readonly ILogger<PowerAssingerController> logger;
        private readonly IHubContext<AssingmentsHub> hub;
        private readonly PowerRequestSolver powerRequestSolver;
        public PowerAssingerController(IHubContext<AssingmentsHub> hub, 
            ILogger<PowerAssingerController> logger, ILogger<PowerRequestSolver> solverLogger)
        {
            this.hub = hub;
            this.logger = logger;
            this.powerRequestSolver = new PowerRequestSolver(solverLogger);
        }

        // GET: api/powerAssinger
        public IActionResult Get()
        {
            return Ok(new { Message = "Request Completed" });
        }

        // POST: api/powerAssinger
        [HttpPost]
        public IActionResult Post([FromBody] PowerRequest powerRequest)
        {
            try
            {
                LogPowerRequest(powerRequest);
                Assingment[] assingments = powerRequestSolver.Solve(powerRequest);
                LogAssingments(assingments);
                BroadcastRequestAssingments(powerRequest, assingments);
                return new JsonResult(assingments);
            }
            catch(Exception ex)
            {
                logger.LogInformation(LoggingEvents.ErrorWhileSolving, 
                    "An error occured while solving a power request,  error message: {0}, HResult: {1}", 
                    ex.Message, ex.HResult);
                return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
        }

        private void BroadcastRequestAssingments(PowerRequest powerRequest, Assingment[] assingments)
        {
            hub.Clients.All.SendAsync("transferRequestAssingments",
                new RequestAssingments(powerRequest, assingments));
        }

        private void LogPowerRequest( PowerRequest powerRequest)
        {
            logger.LogInformation(LoggingEvents.PowerRequestReceived, "New Power request: {powerRequest}",
                 JsonConvert.SerializeObject(powerRequest));
        }

        private void LogAssingments(Assingment[] assingments)
        {
            logger.LogInformation("Assingments: {assingments}", JsonConvert.SerializeObject(assingments));
        }
    }
}
