using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerAssinger.Model
{
    public static class LoggingEvents
    {
        public readonly static int PowerRequestReceived = 2000;
        public readonly static int PerfectSolutionFound = 2001;
        public readonly static int SurplusSolutionFound = 3002;
        public readonly static int DeficientSolutionFound = 3002;
        public readonly static int AssingmentsSent = 2003;

        public readonly static int ErrorWhileSolving = 4000;
    }
}
