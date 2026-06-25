using SeamsApp.DTOs.Event;

namespace SeamsApp.Interfaces.Services.Commands
{
    public interface IEventService
    {
        Task<EventRequest> CreateEventAsync (EventRequest request);
        Task<int> UpdateEventAsync (int eventId, EventRequest request);
        Task<int> DeleteEventAsync(int eventId);
        Task<IEnumerable<EventResponse>> GetAllEventAsync();
        Task<EventResponse> GetEventByIdAsync(int eventId);
    }
}
