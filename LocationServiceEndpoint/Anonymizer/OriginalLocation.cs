using System;

namespace LocationServiceEndpoint.Anonymizer
{
    public class OriginalLocation
    {
        public string Id { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }

        public int K { get; set; }

        public double Dx { get; set; }

        public double Dy { get; set; }

        public long Timestamp { get; set; }
    }
}
