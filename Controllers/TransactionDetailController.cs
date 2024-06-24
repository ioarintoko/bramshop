using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TransactionDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDetail>>> GetTransactionDetails()
        {
            return await _context.TransactionDetails.ToListAsync();
        }

        // GET: api/TransactionDetails/5
        [HttpGet("{id}")]
        public async Task<List<TransactionDetail>> GetTransactionDetail(int id)
        {
            var transactionDetail = await _context.TransactionDetails.Where(td => td.TransactionId == id).ToListAsync();

            return transactionDetail;
        }

        // PUT: api/TransactionDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionDetail(int id, TransactionDetail transactionDetail)
        {
            if (id != transactionDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionDetailExists(id))
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

        // POST: api/TransactionDetails
        [HttpPost]
        public async Task<ActionResult<TransactionDetail>> PostTransactionDetail(TransactionDetail transactionDetail)
        {
            _context.TransactionDetails.Add(transactionDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionDetail", new { id = transactionDetail.Id }, transactionDetail);
        }

        // DELETE: api/TransactionDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionDetail(int id)
        {
            var transactionDetail = await _context.TransactionDetails.FindAsync(id);
            if (transactionDetail == null)
            {
                return NotFound();
            }

            _context.TransactionDetails.Remove(transactionDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionDetailExists(int id)
        {
            return _context.TransactionDetails.Any(e => e.Id == id);
        }
    }
}
