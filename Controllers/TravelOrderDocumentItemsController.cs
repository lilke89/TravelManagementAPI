using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TravelManagementApi.Models.TravelOrder;
using TravelManagementApi.Models.TravelOrderDocument;

namespace TravelManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelOrderDocumentItemsController : ControllerBase
    {
        private readonly TravelOrderDocumentContext _context;

        public TravelOrderDocumentItemsController(TravelOrderDocumentContext context)
        {
            _context = context;
        }

        // GET: api/TravelOrderDocumentItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelOrderDocumentItem>>> GetTravelOrderDocumentItems()
        {
            return await _context.TravelOrderDocumentItems.ToListAsync();
        }

        // GET: api/TravelOrderDocumentItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTravelOrderDocumentItem(long id)
        {
            var travelOrderDocumentItem = await _context.TravelOrderDocumentItems.FindAsync(id);

            if (travelOrderDocumentItem == null)
            {
                return NotFound();
            }

            var documentByteArray = await System.IO.File.ReadAllBytesAsync(travelOrderDocumentItem.Path);
            return File(documentByteArray, MimeTypes.GetMimeType(travelOrderDocumentItem.Name), travelOrderDocumentItem.Name);
        }

        // PUT: api/TravelOrderDocumentItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelOrderDocumentItem(long id, TravelOrderDocumentItem travelOrderDocumentItem)
        {
            if (id != travelOrderDocumentItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(travelOrderDocumentItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TravelOrderDocumentItemExists(id))
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

        // POST: api/TravelOrderDocumentItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TravelOrderDocumentItem>> PostTravelOrderDocumentItem(TravelOrderDocumentItem travelOrderDocumentItem)
        {
            _context.TravelOrderDocumentItems.Add(travelOrderDocumentItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTravelOrderDocumentItem", new { id = travelOrderDocumentItem.Id }, travelOrderDocumentItem);
        }

        // DELETE: api/TravelOrderDocumentItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TravelOrderDocumentItem>> DeleteTravelOrderDocumentItem(long id)
        {
            var travelOrderDocumentItem = await _context.TravelOrderDocumentItems.FindAsync(id);
            if (travelOrderDocumentItem == null)
            {
                return NotFound();
            }

            _context.TravelOrderDocumentItems.Remove(travelOrderDocumentItem);
            await _context.SaveChangesAsync();

            return travelOrderDocumentItem;
        }

        private bool TravelOrderDocumentItemExists(long id)
        {
            return _context.TravelOrderDocumentItems.Any(e => e.Id == id);
        }
    }
}
