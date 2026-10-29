namespace SeamsApp.DTOs.Event
{
    public class EventRequest
    {
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> TargetYearLevels { get; set; } = new();
    }
}
