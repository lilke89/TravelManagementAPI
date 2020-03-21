using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelManagementApi.Models;

namespace TravelManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelOrderItemController : ControllerBase
    {
        private readonly TravelOrderContext _context;

        public TravelOrderItemController(TravelOrderContext context)
        {
            _context = context;
        }

        // GET: api/TravelOrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelOrderItem>>> GetTravelOrderItems()
        {
            return await _context.TravelOrderItems.ToListAsync();
        }

        // GET: api/TravelOrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelOrderItem>> GetTravelOrderItem(long id)
        {
            var travelOrderItem = await _context.TravelOrderItems.FindAsync(id);

            if (travelOrderItem == null)
            {
                return NotFound();
            }

            return travelOrderItem;
        }

        // PUT: api/TravelOrderItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelOrderItem(long id, TravelOrderItem travelOrderItem)
        {
            if (id != travelOrderItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(travelOrderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelOrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TravelOrderItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TravelOrderItem>> PostTravelOrderItem([FromForm]TravelOrderItemDTO travelOrderItemDTO)
        {
            var travelOrderItem = new TravelOrderItem
            {
                Name = travelOrderItemDTO.Document.FileName
            };

            _context.TravelOrderItems.Add(travelOrderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTravelOrderItem), new { id = travelOrderItem.Id }, travelOrderItem);
        }

        // DELETE: api/TravelOrderItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TravelOrderItem>> DeleteTravelOrderItem(long id)
        {
            var travelOrderItem = await _context.TravelOrderItems.FindAsync(id);
            if (travelOrderItem == null)
            {
                return NotFound();
            }

            _context.TravelOrderItems.Remove(travelOrderItem);
            await _context.SaveChangesAsync();

            return travelOrderItem;
        }

        private bool TravelOrderItemExists(long id)
        {
            return _context.TravelOrderItems.Any(e => e.Id == id);
        }
    }
}
