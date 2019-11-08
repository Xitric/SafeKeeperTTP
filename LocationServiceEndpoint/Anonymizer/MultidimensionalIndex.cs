using System.Collections.Generic;
using System.Linq;

namespace LocationServiceEndpoint.Anonymizer
{
    public class MultidimensionalIndex
    {
        private readonly List<OriginalLocation> _messages = new List<OriginalLocation>();

        public void Add(OriginalLocation location)
        {
            _messages.Add(location);
        }

        public void Remove(OriginalLocation location)
        {
            _messages.Remove(location);
        }

        public IEnumerable<OriginalLocation> RangeSearch(BoundingBox bbox)
        {
            return _messages.Where(location => bbox.Contains(new Point { X = location.Lat, Y = location.Lon }));
        }
    }
}
