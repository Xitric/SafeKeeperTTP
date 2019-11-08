using System;
using System.Collections.Generic;
using System.Linq;

namespace LocationServiceEndpoint.Anonymizer
{
    public class ExpirationHeap
    {
        private const long ExpirationTime = 1000 * 60;

        private readonly List<OriginalLocation> _messages = new List<OriginalLocation>();
        private readonly IOrderedEnumerable<OriginalLocation> _sortedMessages; //TODO: Don't actually know if this will even work

        public ExpirationHeap()
        {
            _sortedMessages = _messages.OrderBy(message => message.Timestamp);
        }

        public void Add(OriginalLocation location)
        {
            _messages.Add(location);
        }

        public void Remove(OriginalLocation location)
        {
            _messages.Remove(location);
        }

        public bool isTopExpired()
        {
            long currentTime = DateTime.Now.Millisecond;
            return _sortedMessages.Last().Timestamp + ExpirationTime > currentTime;
        }

        public OriginalLocation getTop()
        {
            return _sortedMessages.Last();
        }
    }
}
