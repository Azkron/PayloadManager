
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PowerAssinger.Services;
using PowerAssinger.Model;
using System.Collections.Generic;
using static PowerAssinger.Services.PowerRequestSolver;

namespace PowerAssinger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerAssingerController : ControllerBase
    {
        private readonly ILogger<PowerAssingerController> _logger;
        public PowerAssingerController(ILogger<PowerAssingerController> logger)
        {
            _logger = logger;
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
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PowerAssinger
        [HttpPost]
        public Assingment[] Post([FromBody] PowerRequest payload)
        {
            return Solve(payload);
            //return new JsonResult(results);
        }


        // PUT: api/Payload/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
