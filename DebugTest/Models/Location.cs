namespace DebugTest.Models
{
    public class Location
    {
        public string title { get; set; }
        public string location_type { get; set; }
        public string latt_long { get; set; }
        public int woeid { get; set; }
        public int? distance { get; set; }
    }
}