using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Data;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Retrieve IdUser from JWT claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user claim.");
            }

            // Query database for cart items of the authenticated user
            var cartItems = _context.Carts.Where(c => c.IdUser == userId).ToList();
            return Ok(cartItems);
        }
        
        [HttpPost]
        public IActionResult AddToCart([FromBody] Cart cart)
        {
            // Get the user ID from the JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new ApplicationException("User identifier not found.");
            }

            // You can use userIdClaim directly as string for other purposes
            // For filtering in database, parse it to integer (if needed)
            var userId = int.Parse(userIdClaim); // Example: Parsing to integer for filtering

            var existingCart = _context.Carts.FirstOrDefault(c => c.IdProduct == cart.IdProduct && c.IdUser == userId);
            if (existingCart != null)
            {
                // Update quantity if product already in cart
                existingCart.Quantity = cart.Quantity;
                existingCart.Price = existingCart.Quantity*cart.Price;
                _context.Carts.Update(existingCart);
            }
            else
            {
                // Add new item to cart
                cart.IdUser = userId;
                _context.Carts.Add(cart);
            }

            _context.SaveChanges();
            return Ok();
        }

     // DELETE: api/cart/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Get the user ID from the JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(); // Return 401 Unauthorized if user ID is not found or not valid
            }

            // Find the cart item by id and userId
            var cartItem = _context.Carts.FirstOrDefault(c => c.Id == id && c.IdUser == userId);
            if (cartItem == null)
            {
                return NotFound(); // Return 404 Not Found if cart item not found
            }

            // Remove the cart item from database
            _context.Carts.Remove(cartItem);
            _context.SaveChanges();

            return NoContent(); // Return 204 No Content on successful deletion
        }

    }
}
