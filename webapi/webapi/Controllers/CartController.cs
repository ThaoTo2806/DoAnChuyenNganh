using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Model;
using System.Text;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _cartDirectory = @"D:\HOCTAP\HK7\DoAn1Chi\DoAnChuyenNganh\data";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);


                var cart = LoadCartFromXml(userId);

                if (cart == null)
                {
                    return NotFound("Cart not found");
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Cart/total-quantity
        [HttpGet("total-quantity")]
        public async Task<IActionResult> GetCartTotalQuantity()
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var cart = LoadCartFromXml(userId);

                if (cart == null)
                {
                    return NotFound("Cart not found");
                }

                var totalQuantity = cart.Items.Sum(p => p.Quantity);
                return Ok(new { TotalQuantity = totalQuantity });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartAddRequest request)
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var product = await _context.Products.FindAsync(request.ProductId);
                var version = await _context.ProductVersions.FindAsync(product.IdVersion);

                if (product == null || product.IsDeleted)
                {
                    return NotFound("Product not found or is deleted");
                }

                var cart = LoadCartFromXml(userId);

                var existingProduct = cart.Items.FirstOrDefault(p => p.ProductId == request.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity += request.Quantity;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = request.ProductId,
                        ProductName = product.Name,
                        image = product.Image,
                        Price = product.Price,
                        Quantity = request.Quantity,
                        tenVersion = version.Version,
                        giaVersion = version.Price
                    });
                }

                SaveCartToXml(userId, cart);

                return Ok("Product added to cart");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Cart/update
        [HttpPost("update")]
        public async Task<IActionResult> UpdateCart([FromBody] CartUpdateRequest request)
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var cart = LoadCartFromXml(userId);

                var existingProduct = cart.Items.FirstOrDefault(p => p.ProductId == request.ProductId);
                if (existingProduct == null)
                {
                    return NotFound("Product not found in cart");
                }

                existingProduct.Quantity = request.Quantity;

                SaveCartToXml(userId, cart);

                return Ok("Cart updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        // POST: api/Cart/remove
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart([FromBody] CartRemoveRequest request)
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var cart = LoadCartFromXml(userId);

                var existingProduct = cart.Items.FirstOrDefault(p => p.ProductId == request.ProductId);
                if (existingProduct == null)
                {
                    return NotFound("Product not found in cart");
                }

                cart.Items.Remove(existingProduct);

                SaveCartToXml(userId, cart);

                return Ok("Product removed from cart");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Cart/clear
        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var cart = new Cart(); // Tạo một giỏ hàng rỗng
                SaveCartToXml(userId, cart);

                return Ok("Cart cleared successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Cart/increment
        [HttpPost("increment")]
        public async Task<IActionResult> IncrementQuantity([FromBody] ProductIdRequest request)
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var cart = LoadCartFromXml(userId);

                var existingProduct = cart.Items.FirstOrDefault(p => p.ProductId == request.ProductId);
                if (existingProduct == null)
                {
                    return NotFound("Product not found in cart");
                }

                existingProduct.Quantity += 1;

                SaveCartToXml(userId, cart);

                return Ok("Product quantity incremented successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Cart/decrement
        [HttpPost("decrement")]
        public async Task<IActionResult> DecrementQuantity([FromBody] ProductIdRequest request)
        {
            try
            {
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    return Unauthorized("User is not logged in");
                }

                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                var cart = LoadCartFromXml(userId);

                var existingProduct = cart.Items.FirstOrDefault(p => p.ProductId == request.ProductId);
                if (existingProduct == null)
                {
                    return NotFound("Product not found in cart");
                }

                if (existingProduct.Quantity > 1)
                {
                    existingProduct.Quantity -= 1;
                }
                else
                {
                    // Remove the product if quantity is 0
                    cart.Items.Remove(existingProduct);
                }

                SaveCartToXml(userId, cart);

                return Ok("Product quantity decremented successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        public class ProductIdRequest
        {
            public string ProductId { get; set; }
        }


        private void SaveCartToXml(int userId, Cart cart)
        {
            try
            {
                if (!Directory.Exists(_cartDirectory))
                {
                    Directory.CreateDirectory(_cartDirectory);
                }

                var cartXmlPath = Path.Combine(_cartDirectory, $"{userId}_cart.xml");
                var xmlSerializer = new XmlSerializer(typeof(Cart));
                using (var writer = new StreamWriter(cartXmlPath))
                {
                    xmlSerializer.Serialize(writer, cart);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cart to XML: {ex.Message}");
            }
        }

        private Cart LoadCartFromXml(int userId)
        {
            var cartXmlPath = Path.Combine(_cartDirectory, $"{userId}_cart.xml");
            try
            {
                if (System.IO.File.Exists(cartXmlPath))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Cart));
                    using (var reader = new StreamReader(cartXmlPath))
                    {
                        return (Cart)xmlSerializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cart from XML: {ex.Message}");
            }

            return new Cart(); // Return an empty cart if the file does not exist
        }
    }
}
