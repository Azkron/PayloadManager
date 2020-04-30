using PayloadManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayloadManager.Components
{
    public static class PayloadProcessor
    {
        private static Payload _payload;
        private static Dictionary<string,
            Func<Powerplant, Dictionary<string, float>, PowerplantInfo>> _infoBuilders;
        private static float _minAverageCost = 9999f;
        private static List<Node> _nodes;
        private static Node _lastNode;
        private static bool _optimalFound;
        public static void _Init()
        {
            _infoBuilders = new Dictionary<string,
                Func<Powerplant, Dictionary<string, float>, PowerplantInfo>>();

            _infoBuilders["gasfired"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerplantInfo info = new PowerplantInfo(plant);
                info.costPerUnit = fuels["gas(euro/MWh)"] * (1f / plant.efficiency);
                info.costPerUnit += fuels["co2(euro/ton)"] * 0.3f;
                return info;
            };

            _infoBuilders["turbojet"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerplantInfo info = new PowerplantInfo(plant);
                info.costPerUnit = fuels["kerosine(euro/MWh)"] * (1f / plant.efficiency);
                return info;
            };

            _infoBuilders["windturbine"] = (Powerplant plant, Dictionary<string, float> fuels) =>
            {
                PowerplantInfo info = new PowerplantInfo(plant);
                info.pmax = (int)(plant.pmax * (fuels["wind(%)"] / 100f));
                return info;
            };
        }

        public static Assingment[] Process(Payload payload)
        {
            _payload = payload;

            List<PowerplantInfo> infos = new List<PowerplantInfo>();
            foreach (Powerplant p in payload.powerPlants)
                infos.Add(_infoBuilders[p.type](p, payload.fuels));

            infos.Sort((p1, p2) => p1.costPerUnit < p2.costPerUnit ? -1 : 1);

            Node node = AstarSearch(infos);

            if (node != null)
                return node.GetAssingments();
            return new Assingment[] { new Assingment() { name = "no", p = 0 } };
        }
        private static Node AstarSearch(List<PowerplantInfo> infos)
        {
            _nodes = new List<Node>();
            _nodes.Add(new Node(infos));
            while (_nodes.Count > 0 && !_optimalFound)
            {
                _nodes.Sort(Node.Comparer);
                Node node = _nodes[0];
                if (node.done)
                    return node;
                _nodes.RemoveAt(0);
                _minAverageCost = node.averageCost;
                node.Explore();
            }

            // TODO return node based on done, surpass quantity, average
            return null;
        }

        private class Node
        {
            List<Node> connections;
            List<PowerplantInfo> unused = new List<PowerplantInfo>();
            List<PowerplantInfo> used = new List<PowerplantInfo>();
            List<int> ps = new List<int>();
            int totalP;
            float totalCost;
            bool surpassed;
            public float averageCost { get; private set; }
            public bool done { get; private set; }


            public Node(List<PowerplantInfo> unused)
            {
                this.unused.AddRange(unused);
            }

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
                int pNeeded = _payload.load - totalP;
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

                done = totalP == _payload.load;
                surpassed = totalP > _payload.load;
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

            public void Explore()
            {
                BuildConnections();
                foreach (Node node in connections)
                    _nodes.Add(node);
            }

            private void BuildConnections()
            {
                connections = new List<Node>();
                for (int i = 0; i < unused.Count; ++i)
                {
                    Node node = new Node(this, i);
                    if (!node.surpassed)
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

        public struct Assingment
        {
            public string name { get; set; }
            public int p { get; set; }
        }
    }

}
