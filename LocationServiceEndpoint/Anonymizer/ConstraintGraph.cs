using System.Collections.Generic;
using System.Linq;

namespace LocationServiceEndpoint.Anonymizer
{
    public class ConstraintGraph
    {
        private readonly List<Node> _nodes = new List<Node>();

        public void AddNode(OriginalLocation message)
        {
            _nodes.Add(new Node(message));
        }

        public void RemoveNode(OriginalLocation message)
        {
            //Remove all edges going into the node
            foreach (var node in _nodes)
            {
                node.Edges = node.Edges.Where(edge => edge.To != message).ToList();
            }

            //Remove the node itself
            _nodes.Remove(_nodes.Find(node => node.Message == message));
        }

        public void AddEdge(OriginalLocation from, OriginalLocation to)
        {
            var node = _nodes.Find(n => n.Message.Equals(from));
            node.Edges.Add(new Edge(from, to));
        }

        public ConstraintGraph SubGraph(IEnumerable<OriginalLocation> n)
        {
            var result = new ConstraintGraph();

            foreach (var node in _nodes.Where(node => n.Contains(node.Message)))
            {
                result.AddNode(node.Message);

                foreach (var edge in node.Edges.Where(edge => n.Contains(edge.To)))
                {
                    result.AddEdge(edge.From, edge.To);
                }
            }

            return result;
        }

        public IEnumerable<OriginalLocation> Nbr(OriginalLocation ms)
        {
            return _nodes.Find(m => m.Message == ms).Edges.Select(edge => edge.To).ToList();
        }

        //Credit to myself for this one :P
        public IEnumerable<OriginalLocation> FindClique(OriginalLocation msc, int k)
        {
            var mscNode = _nodes.Find(node => node.Message == msc);
            var clique = new List<OriginalLocation>();

            foreach (var mscNeighbor in mscNode.Edges.Select(mscEdge => mscEdge.To))
            {
                clique.Clear();
                clique.Add(msc);
                clique.Add(mscNeighbor);
                ConsiderNeighborsTo(mscNeighbor, clique);

                if (clique.Count >= k) return clique;
            }

            return new List<OriginalLocation>();
        }

        private void ConsiderNeighborsTo(OriginalLocation ms, List<OriginalLocation> clique)
        {
            var msNode = _nodes.Find(node => node.Message == ms);
            foreach (var msNeighbor in msNode.Edges.Select(mscEdge => mscEdge.To))
            {
                if (clique.Contains(msNeighbor)) continue;

                var msNeighborNode = _nodes.Find(node => node.Message == msNeighbor);
                if (clique.Any(c => !msNeighborNode.Edges.Select(e => e.To).Contains(c))) continue;

                clique.Add(msNeighbor);
                ConsiderNeighborsTo(msNeighbor, clique);
            }
        }
    }

    public class Node
    {
        public OriginalLocation Message;
        public List<Edge> Edges;

        public Node(OriginalLocation message)
        {
            Message = message;
            Edges = new List<Edge>();
        }
    }

    public class Edge
    {
        public OriginalLocation From;
        public OriginalLocation To;

        public Edge(OriginalLocation from, OriginalLocation to)
        {
            From = from;
            To = to;
        }
    }
}
