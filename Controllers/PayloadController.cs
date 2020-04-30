
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayloadManager.Components;
using PayloadManager.Model;
using System.Collections.Generic;
using static PayloadManager.Components.PayloadSolver;

namespace PayloadManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayloadController : ControllerBase
    {
        private readonly ILogger<PayloadController> _logger;
        public PayloadController(ILogger<PayloadController> logger)
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

        // POST: api/Payload
        [HttpPost]
        public Assingment[] Post([FromBody] Payload payload)
        {
            return Process(payload);
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
