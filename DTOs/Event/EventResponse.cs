namespace SeamsApp.DTOs.Event
{
    public class EventResponse
    {
        public int EventId { get; set; }
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
