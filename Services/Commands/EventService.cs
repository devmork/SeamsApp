using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeamsApp.Data;
using SeamsApp.DTOs.Event;
using SeamsApp.Interfaces.Services.Commands;
using SeamsApp.Models;
using SeamsApp.Utilities;

namespace SeamsApp.Services.Commands
{
    public class EventService : IEventService
    {
        private readonly SeamsDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventService(SeamsDbContext dbContext,
                            IMapper mapper,
                            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<EventRequest> CreateEventAsync(EventRequest request)
        {
            var userId = ClaimsUtility.GetUserIdFromClaims(_httpContextAccessor.HttpContext!);

            var newEvent = _mapper.Map<Event>(request);
            newEvent.Status = 1;
            newEvent.CreatedBy = userId;

            await _dbContext.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<EventRequest>(newEvent);
        }

        public async Task<int> DeleteEventAsync(int eventId)
        {
            var existingEvent = await _dbContext.Events.FindAsync(eventId);
            if (existingEvent == null)
            {
                return 0;
            }

            existingEvent.Status = 0;
            _dbContext.Events.Update(existingEvent);
            return _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<EventResponse>> GetAllEventAsync()
        {
            var events = await _dbContext.Events
                                .Where(e => e.Status == 1)
                                .ToListAsync();

            var response = _mapper.Map<IEnumerable<EventResponse>>(events);
            return response;
        }

        public async Task<EventResponse> GetEventByIdAsync(int eventId)
        {
            var existingEvent = await _dbContext.Events
                                        .FirstOrDefaultAsync(e => e.EventId == eventId && e.Status == 1);

            var response = _mapper.Map<EventResponse>(existingEvent);
            return response;
        }

        public async Task<int> UpdateEventAsync(int eventId, EventRequest request)
        {
            var existingEvent = await _dbContext.Events.FindAsync(eventId);
            if (existingEvent == null)
            {
                return 0;
            }

            existingEvent.Title = request.Title;
            existingEvent.StartDate = request.StartDate;
            existingEvent.EndDate = request.EndDate;

            _dbContext.Events.Update(existingEvent);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
