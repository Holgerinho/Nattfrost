using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using NattfrostBackend.Data;


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
    }
}
