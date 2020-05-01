using System;
using System.Collections.Generic;
using static PowerAssinger.Services.PowerRequestSolver;

namespace PowerAssinger.Model
{
    public class RequestAssingments
    {
        public PowerRequest powerRequest { get; set; }
        public Assingment[] assingments { get; set; }

        public RequestAssingments(PowerRequest powerRequest, Assingment[] assingments)
        {
            this.powerRequest = powerRequest;
            this.assingments = assingments;
        }
    }
}