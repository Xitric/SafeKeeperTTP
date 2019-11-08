namespace LocationServiceEndpoint.Anonymizer
{
    public class BoundingBox
    {
        public double MinX { get; set; }

        public double MaxX { get; set; }

        public double MinY { get; set; }

        public double MaxY { get; set; }

        public bool Contains(Point point)
        {
            return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
        }
    }
}
