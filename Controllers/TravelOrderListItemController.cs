using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelManagementApi.Models;
using TravelManagementApi.Models.TravelOrderList;

namespace TravelManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelOrderListItemController : ControllerBase
    {
        private readonly TravelOrderListContext _context;
        IWebHostEnvironment _webHostingEnvironment;


        public TravelOrderListItemController(IWebHostEnvironment webHostingEnvironment, TravelOrderListContext context)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _context = context;

        }

        // GET: api/TravelOrderListItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelOrderListItem>>> GetTravelOrderListItems()
        {
            return await _context.TravelOrderListItems.ToListAsync();
        }

        // GET: api/TravelOrderListItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TravelOrderListItem>> GetTravelOrderListItem(long id)
        {
            var travelOrderItem = await _context.TravelOrderListItems.FindAsync(id);

            if (travelOrderItem == null)
            {
                return NotFound();
            }

            return travelOrderItem;
        }

        // PUT: api/TravelOrderListItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTravelOrderListItem(long id, TravelOrderListItem travelOrderItem)
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

        // POST: api/TravelOrderListItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TravelOrderListItem>> PostTravelOrderListItem([FromForm(Name = "document")] IFormFile traveloOrderListItem)
        {
            var travelOrderListItemManager = new TravelOrderListItemManager(traveloOrderListItem);

            var uploads = Path.Combine(_webHostingEnvironment.ContentRootPath, "uploads/spreadsheets");
            var filePath = Path.Combine(uploads, travelOrderListItemManager.ListName);

            var travelOrderListItem = await travelOrderListItemManager.SaveListAsync(filePath);


            _context.TravelOrderListItems.Add(travelOrderListItem);
            await _context.SaveChangesAsync();

            var travelOrderDataItems = travelOrderListItemManager.GetExtractedListData();

            return CreatedAtAction(nameof(GetTravelOrderListItem), new { id = travelOrderListItem.Id }, travelOrderListItem);

        }

        // DELETE: api/TravelOrderListItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TravelOrderListItem>> DeleteTravelOrderListItem(long id)
        {
            var travelOrderItem = await _context.TravelOrderListItems.FindAsync(id);
            if (travelOrderItem == null)
            {
                return NotFound();
            }

            _context.TravelOrderListItems.Remove(travelOrderItem);
            await _context.SaveChangesAsync();

            return travelOrderItem;
        }

        private bool TravelOrderItemExists(long id)
        {
            return _context.TravelOrderListItems.Any(e => e.Id == id);
        }
    }
}
