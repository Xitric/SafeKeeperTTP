namespace LocationServiceEndpoint.Anonymizer
{
    public class AnonymizedLocation
    {
        public string Id { get; set; }

        public double LatMin { get; set; }

        public double LatMax { get; set; }

        public double LonMin { get; set; }

        public double LonMax { get; set; }
    }
}
