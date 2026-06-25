using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using SeamsApp.DTOs.Event;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Services.Commands;

namespace SeamsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [OutputCache]
        [Authorize(Roles ="Admin, Officer, Student")]
        [ProducesResponseType(typeof(EventRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventAsync();
            if (events == null)
            {
                return NotFound();
            }

            return Ok(events);
        }

        [HttpGet("{eventId}")]
        [Authorize(Roles = "Admin, Officer")]
        [OutputCache]
        [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventResponse>> GetEventById(int eventId)
        {
            var existingEvent = await _eventService.GetEventByIdAsync(eventId);
            if (existingEvent == null)
            {
                return NotFound();
            }

            return Ok(existingEvent);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(EventRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventRequest>> CreateEvent([FromBody]EventRequest eventRequest)
        {
            var newEvent = await _eventService.CreateEventAsync(eventRequest);
            if (newEvent == null)
            {
                return BadRequest();
            }
            return Ok(newEvent);
        }

        [HttpPut("{eventId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(EventRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> UpdateEvent(int eventId, [FromBody] EventRequest eventRequest)
        {
            var eventToUpdate = await _eventService.UpdateEventAsync(eventId, eventRequest);
            if (eventId! == 0)
            {
                return NoContent();
            }

            return Ok(eventToUpdate);
        }


        [HttpDelete("{eventId:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> DeleteEvent(int eventId)
        {
            var eventToDelete = await _eventService.DeleteEventAsync(eventId);
            if (eventId == 0)
            {
                return NotFound();
            }

            return Ok(eventToDelete);
        }
    }
}
