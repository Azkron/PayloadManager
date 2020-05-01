
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PowerAssinger.Services;
using PowerAssinger.Model;
using System.Collections.Generic;
using static PowerAssinger.Services.PowerRequestSolver;
using Microsoft.AspNetCore.SignalR;
using PowerAssinger.HubConfig;

namespace PowerAssinger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerAssingerController : ControllerBase
    {
        private readonly ILogger<PowerAssingerController> _logger;
        private readonly IHubContext<AssingmentsHub> _hub;
        private RequestAssingments _requestAssingments;
        public PowerAssingerController(IHubContext<AssingmentsHub> hub, ILogger<PowerAssingerController> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        // GET: api/powerAssinger
        public IActionResult Get()
        {

            return Ok(new { Message = "Request Completed" });
        }

        // POST: api/powerAssinger
        [HttpPost]
        public Assingment[] Post([FromBody] PowerRequest powerRequest)
        {
            Assingment[] assingments = Solve(powerRequest);
            _requestAssingments = new RequestAssingments(powerRequest, assingments);
            _hub.Clients.All.SendAsync("transferRequestAssingments", _requestAssingments);
            return assingments;
            //return new JsonResult(results);
        }


        // GET: api/Payload
        //[HttpGet]
        //public PowerplantAssingment[] Get()
        //{
        //    return _payloadRepository.GetPowerplantsAssignments();
        //}

        // GET: api/Payload
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Payload/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// PUT: api/Payload/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

    }
}
