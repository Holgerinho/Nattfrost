using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using NattfrostBackend.Data;
using NattfrostBackend.DTOs;
using System.Reflection.Metadata;
using NattfrostBackend.Entities;


namespace NattfrostBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SubscriberController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscriber(CreateSubscribeRequest requesten)
        {
            // Modelvalidation is checked by ASP.Net Core.
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSubscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == requesten.Email);
            if (existingSubscriber != null)
            {
                return BadRequest("Email already subscribed");
            }

            var subscriber = new Subscriber
            {
                Email = requesten.Email,
                City = requesten.City,
                CreatedAt = DateTime.UtcNow
            };
            
            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();
            return Ok(new {Message = "Subscriber created successfully." }); // DONE!
        
        }// end CreateSubscriber
    

        [HttpGet]
        public async Task<IActionResult> GetSubscribers()
        {
            var subscribers = await _context.Subscribers.ToListAsync();

            return Ok(subscribers);
        }

    }// end class SubscriberController
}
