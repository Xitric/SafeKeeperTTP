using System;
using System.Collections.Generic;
using System.Linq;

namespace LocationServiceEndpoint.Anonymizer
{
    public class Anonymizer
    {
        private MultidimensionalIndex _im = new MultidimensionalIndex();
        private ExpirationHeap _hm = new ExpirationHeap();
        private ConstraintGraph _gm = new ConstraintGraph();

        public AnonymizedLocation Anonymize(OriginalLocation msc)
        {
            _im.Add(msc);
            _hm.Add(msc);
            _gm.AddNode(msc);

            var n = _im.RangeSearch(Bcn(msc)).ToList();
            foreach (var ms in n.Where(ms => ms != msc))
            {
                if (Bcn(ms).Contains(new Point { X = msc.Lat, Y = msc.Lon }))
                {
                    //Add two edges to simulate a unidirectional graph
                    _gm.AddEdge(msc, ms);
                    _gm.AddEdge(ms, msc);
                }
            }

            var gmMark = _gm.SubGraph(n);
            var m = LocalKSearch(msc.K, msc, gmMark).ToList();

            if (!m.Any()) return null;

            _im.Remove(msc);
            _hm.Remove(msc);
            _gm.RemoveNode(msc);
            var bbox = Mbr(m);
            return new AnonymizedLocation
            {
                Id = msc.Id,
                LatMin = bbox.MinX,
                LatMax = bbox.MaxX,
                LonMin = bbox.MinY,
                LonMax = bbox.MaxY
            };
        }

        private static BoundingBox Bcn(OriginalLocation m)
        {
            return new BoundingBox
            {
                MinX = m.Lat - m.Dx,
                MaxX = m.Lat + m.Dx,
                MinY = m.Lon - m.Dy,
                MaxY = m.Lon + m.Dy
            };
        }

        private static BoundingBox Mbr(IEnumerable<OriginalLocation> messages)
        {
            var result = new BoundingBox();

            foreach (var ms in messages)
            {
                result.MinX = Math.Min(result.MinX, ms.Lat);
                result.MaxX = Math.Max(result.MaxX, ms.Lat);
                result.MinY = Math.Min(result.MinY, ms.Lon);
                result.MaxY = Math.Max(result.MaxY, ms.Lon);
            }

            return result;
        }

        private static IEnumerable<OriginalLocation> LocalKSearch(int k, OriginalLocation msc, ConstraintGraph gmMark)
        {
            var u = gmMark.Nbr(msc).Where(ms => ms.K <= k).ToList();
            if (u.Count() < k - 1) return new List<OriginalLocation>();

            var l = 0;
            while (l != u.Count())
            {
                l = u.Count();
                foreach (var ms in new List<OriginalLocation>(u))
                {
                    if (gmMark.Nbr(ms).Count() < k - 2)
                    {
                        u.Remove(ms);
                        gmMark.RemoveNode(ms);
                    }
                }
            }

            return gmMark.FindClique(msc, k);
        }
    }
}
