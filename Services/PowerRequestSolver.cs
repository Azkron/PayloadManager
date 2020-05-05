using Microsoft.Extensions.Logging;
using PowerAssinger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerAssinger.Services
{
    public partial class PowerRequestSolver
    {
        private PowerRequest powerRequest;
        private List<Node> nodes;
        private List<PowerplantInfo> infos;
        private ILogger<PowerRequestSolver> logger;

        public PowerRequestSolver(ILogger<PowerRequestSolver> logger)
        {
            this.logger = logger;
        }

        public Assingment[] Solve(PowerRequest powerRequest)
        {
            this.powerRequest = powerRequest;
            SetPowerplantsInfos(powerRequest);

            if (!PowerSurplus())
                return MaxAssingments();
            else
                return AstarSearch(infos);
        }

        private void SetPowerplantsInfos(PowerRequest powerRequest)
        {
            infos = new List<PowerplantInfo>();
            foreach (Powerplant p in powerRequest.powerplants)
                infos.Add(new PowerplantInfo(p, powerRequest.fuels));
        }

        private bool PowerSurplus()
        {
            int totalMaxP = 0;
            foreach (PowerplantInfo info in infos)
                totalMaxP += info.pmax;

            if(totalMaxP < powerRequest.load)
                logger.LogInformation(LoggingEvents.DeficientSolutionFound, 
                    "Not enough total avaliable power ({totalMaxP}) to meet load ({powerRequest.load}), a defficient solution will be created anyway", 
                    totalMaxP, powerRequest.load);

            return totalMaxP > powerRequest.load;
        }

        private Assingment[] MaxAssingments()
        {
            Assingment[] assingments = new Assingment[infos.Count];
            for (int i = 0; i < assingments.Length; ++i)
                assingments[i] = new Assingment() { name = infos[i].name, p = infos[i].pmax };
            return assingments;
        }

        private void ResetGraph()
        {
            nodes = new List<Node>();
        }

        private Assingment[] AstarSearch(List<PowerplantInfo> infos)
        {
            infos.Sort((p1, p2) => p1.costPerUnit < p2.costPerUnit ? -1 : 1);
            ResetGraph();
            nodes.Add(new Node(infos, powerRequest));
            Node perfectSolutionNode = null;
            while (nodes.Count > 0)
            {
                nodes.Sort(Node.Comparer);
                Node node = nodes[0];
                if (node.done)
                {
                    perfectSolutionNode = node;
                    break;
                }
                else
                {
                    nodes.RemoveAt(0);
                    nodes.AddRange(node.Explore());
                }
            }


            if (perfectSolutionNode != null)
            {
                logger.LogInformation(LoggingEvents.PerfectSolutionFound,
                    "Perfect power assingment solution found");
                return perfectSolutionNode.GetAssingments();
            }
            else
            {
                int waste = Node._closestP - powerRequest.load;
                logger.LogInformation(LoggingEvents.SurplusSolutionFound,
                    "Best possible solution allocates {Node._closestP} power for a load {powerRequest.load}, wasting {waste} power", Node._closestP, powerRequest.load, waste);
                return Node._lastBestNode.GetAssingments();
            }
        }


        public struct Assingment
        {
            public string name { get; set; }
            public int p { get; set; }
        }
    }

}
