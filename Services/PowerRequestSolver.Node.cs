using PowerAssinger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerAssinger.Services
{
    public partial class PowerRequestSolver
    {
        private class Node
        {
            private static PowerRequest _powerRequest;
            public static int _closestP { get; private set; }
            public static Node _lastBestNode { get; private set; }
            public static float _minAverageCost { get; private set; }

            List<Node> connections;
            List<PowerplantInfo> unused = new List<PowerplantInfo>();
            List<PowerplantInfo> used = new List<PowerplantInfo>();
            List<int> ps = new List<int>();
            int totalP;
            public float totalCost { get; private set; }
            public float averageCost { get; private set; }
            public bool done { get; private set; }

            private static void ResetGrapth()
            {
                _closestP = int.MaxValue;
                _lastBestNode = null;
                _minAverageCost = float.MaxValue;
            }

            // for the initial node
            public Node(List<PowerplantInfo> unused, PowerRequest powerRequest)
            {
                _powerRequest = powerRequest;
                ResetGrapth();
                this.unused.AddRange(unused);
            }

            // for all child nodes
            public Node(Node prev, int nextToUse)
            {
                unused.AddRange(prev.unused);
                used.AddRange(prev.used);
                ps.AddRange(prev.ps);
                totalP = prev.totalP;

                AddNext(unused[nextToUse]);
            }

            private void AddNext(PowerplantInfo info)
            {
                int pNeeded = _powerRequest.load - totalP;
                int p;
                if (pNeeded >= info.pmax)
                    p = info.pmax;
                else if (info.pmin > pNeeded)
                {
                    p = info.pmin;
                    ReducePreviousP(p - pNeeded);
                }
                else
                    p = pNeeded;

                unused.Remove(info);
                used.Add(info);
                ps.Add(p);

                CalculateTotalPAndCost();

                done = totalP == _powerRequest.load;
            }

            private void ReducePreviousP(int toReduce)
            {
                for (int i = used.Count - 1; i >= 0; --i)
                {
                    PowerplantInfo info = used[i];
                    ps[i] -= toReduce;
                    if (ps[i] < info.pmin)
                    {
                        int diff = info.pmin - ps[i];
                        toReduce = diff;
                        ps[i] = info.pmin;
                    }
                    else
                        break;
                }
            }

            private void CalculateTotalPAndCost()
            {
                totalP = 0;
                totalCost = 0;
                for (int i = 0; i < used.Count; ++i)
                {
                    totalP += ps[i];
                    totalCost += used[i].costPerUnit * ps[i];
                }

                averageCost = totalCost / totalP;
            }

            public List<Node> Explore()
            {
                _minAverageCost = averageCost;
                BuildConnections();
                foreach (Node node in connections)
                    if (node.totalP > _powerRequest.load &&
                        (node.totalP < _closestP || (node.totalP == _closestP && node.averageCost < _minAverageCost)))
                    {
                        _closestP = node.totalP;
                        _minAverageCost = node.averageCost;
                        _lastBestNode = node;
                    }

                return connections;
            }

            private void BuildConnections()
            {
                connections = new List<Node>();
                for (int i = 0; i < unused.Count; ++i)
                {
                    Node node = new Node(this, i);
                    connections.Add(node);
                }

                connections.Sort(Comparer);
            }

            public static int Comparer(Node n1, Node n2)
            {
                return n1.averageCost < n2.averageCost ? -1 : 1;
            }

            public Assingment[] GetAssingments()
            {
                List<Assingment> assingments = new List<Assingment>();
                for (int i = 0; i < used.Count; ++i)
                    assingments.Add(new Assingment { name = used[i].name, p = ps[i] });
                foreach (PowerplantInfo info in unused)
                    assingments.Add(new Assingment { name = info.name, p = 0 });

                return assingments.ToArray();
            }
        }
    }

}
