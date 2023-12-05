using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegotiationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NegotiationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Negotiation>>> GetNegotiations()
        {
            var negotiations = await _context.Negotiations.ToListAsync();
            return Ok(negotiations);
        }

        [HttpPost]
        public async Task<ActionResult<Negotiation>> StartNegotiation([FromBody] Negotiation negotiation)
        {
            if (negotiation.ProposedPrice <= 0)
                return BadRequest("Proposed price must be higher than 0");

            if (negotiation.ProposedPrice > 2 * _context.Products.FirstOrDefault(p => p.Id == negotiation.ProductId)?.Price)
                return BadRequest("Proposed price cannot be twice or more than the basic price.");

            negotiation.NegotiationAttempts = 1;
            negotiation.IsAccepted = false;
            negotiation.IsWaitingForResponse = true;

            await _context.Negotiations.AddAsync(negotiation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNegotiations), new { id = negotiation.Id }, negotiation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Negotiation>> AnotherTryNegotiation(int id, [FromBody] Negotiation newTryNegotiation)
        {
            var negotiation = await _context.Negotiations.FirstOrDefaultAsync(n => n.Id == id);

            if (negotiation == null)
                return NotFound("The offer you are looking for does not exit");

            if (newTryNegotiation.ProposedPrice <= 0)
                return BadRequest("Proposed price must be higher than 0");

            if (newTryNegotiation.ProposedPrice > 2 * _context.Products.FirstOrDefault(p => p.Id == negotiation.ProductId)?.Price)
                return BadRequest("Proposed price cannot be twice or more than the basic price.");

            if (negotiation.NegotiationAttempts >= 3)
            {
                negotiation.IsAccepted = false;
                return BadRequest("Sorry, you used your 3 negotiation tries");
            }

            if (negotiation.IsWaitingForResponse)
                return BadRequest("Wait for response before changing your offer");

            if (negotiation.IsAccepted)
                return BadRequest("This offer is already accepted");

            negotiation.ProposedPrice = newTryNegotiation.ProposedPrice;
            negotiation.NegotiationAttempts++;
            negotiation.IsWaitingForResponse = true;

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNegotiations), new { id = negotiation.Id }, negotiation);
        }

        [HttpPut("respond/{id}")]
        public async Task<ActionResult> RespondToNegotiation(int id, [FromBody] bool isAccepted)
        {
            var negotiation = await _context.Negotiations.FirstOrDefaultAsync(n => n.Id == id);

            if (negotiation == null)
                return NotFound();

            if (negotiation.IsAccepted == true)
                return BadRequest("You already accepted this negotiation");

            if (isAccepted)
            {
                negotiation.IsAccepted = true;
                negotiation.IsWaitingForResponse = false;
                await _context.SaveChangesAsync();
            }
            negotiation.IsWaitingForResponse = false;
            await _context.SaveChangesAsync();

            return Ok(negotiation);
        }
    }
}
