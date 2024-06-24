using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMessageQueueService _messageQueueService;

        public TransactionsController(ApplicationDbContext context, IMessageQueueService messageQueueService)
        {
            _context = context;
            _messageQueueService = messageQueueService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

         [HttpGet("user")]
        public async Task<IActionResult> GetTransactionsByUserId()
        {
            // Retrieve IdUser from JWT token
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("User identifier not found or is not a valid integer.");
            }

            // Query transactions for the specified user id
            var transactions = await _context.Transactions
                .Where(t => t.IdUser == userId)
                .ToListAsync();

            return Ok(transactions);
        }

        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            try
        {
            // Get the userId from the JWT
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("User identifier not found or is not a valid integer.");
            }

            // Set the userId in the transaction
            transaction.IdUser = userId;
            transaction.CreateDate = DateTime.Now;
            transaction.Status = "Pending";

            // Add the transaction to the context
            _context.Transactions.Add(transaction);
            
            // Save changes to generate transaction ID
            await _context.SaveChangesAsync();

            // Iterate over transaction details and add them to the context
            foreach (var detail in transaction.TransactionDetails)
            {
                detail.TransactionId = transaction.Id; // Associate detail with the transaction ID
                _context.TransactionDetails.Add(detail);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Retrieve user email (implement this method based on your user model)
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Send email notification using RabbitMQ
            var message = $"Dear {user.Name}, your transaction with id {transaction.Id} has been successfully processed.";
            await _messageQueueService.PublishEmailNotificationAsync(user.Email, message);

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
        }
         public string GetUserEmailFromTransaction(int transactionId)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == transactionId);
            if (transaction != null)
            {
                // Assuming there's a relationship to User table and User has an Email property
                var user = _context.Users.FirstOrDefault(u => u.Id == transaction.IdUser);
                return user?.Email;
            }
            return null; // or throw exception if transaction not found
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
