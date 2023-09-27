using CareerCompassAPI.Application.Abstraction.Services;
using CareerCompassAPI.Application.DTOs.Event_DTOs;
using CareerCompassAPI.Domain.Entities;
using CareerCompassAPI.Persistence.Contexts;
using CareerCompassAPI.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CareerCompassAPI.Persistence.Implementations.Services
{
    public class EventService : IEventService
    {
        private readonly CareerCompassDbContext _context;
        private readonly ILogger<EventService> _logger;

        public EventService(CareerCompassDbContext context, ILogger<EventService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> Create(EventCreateDto eventCreateDto)
        {
            _logger.LogInformation($"Received DTO: {eventCreateDto}");
            if (eventCreateDto is null)
            {
                throw new ArgumentNullException("Arguments passed in may not contain null values");
            }
            var user = await _context.Users.FindAsync(eventCreateDto.userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            Event newEvent = new()
            {
                User = user,
                Title = eventCreateDto.title,
                Start = eventCreateDto.startDate,
                End = eventCreateDto.endDate,
                Importance = eventCreateDto.importance
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            return newEvent.Id;
        }

        public async Task<List<EventGetDto>> GetEventsByUserId(string appUserId)
        {
            if (appUserId is null)
            {
                throw new ArgumentNullException("Argument passed in may not contain null values");
            }
            var events = await _context.Events
             .Where(e => e.User.Id == appUserId)
             .Select(e => new EventGetDto(e.Id, e.Title, e.Start, e.End, e.Importance))
             .ToListAsync();

            return events;
        }

        public async Task UpdateEvent(EventUpdateDto eventUpdateDto)
        {
            var existingEvent = await _context.Events.SingleOrDefaultAsync(e => e.Id == eventUpdateDto.id);
            if (existingEvent != null) { return; }
            existingEvent.Start = eventUpdateDto.start;
            existingEvent.End = eventUpdateDto.end;
            _context.Events.Update(existingEvent);
            _context.SaveChangesAsync();
        }
    }
}
