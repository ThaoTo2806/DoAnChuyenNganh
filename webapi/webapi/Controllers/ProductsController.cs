using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var allProducts = await _context.Products.ToListAsync();
            var notDeletedProducts = allProducts.Where(p => !p.IsDeleted).ToList();

            Console.WriteLine($"Total Products: {allProducts.Count}");
            Console.WriteLine($"Not Deleted Products: {notDeletedProducts.Count}");

            var products = notDeletedProducts
                .Select(p => new Product
                {
                    ID = p.ID,
                    Name = p.Name,
                    IdCate = p.IdCate,
                    Evaluate = p.Evaluate,
                    SL = p.SL,
                    Price = p.Price,
                    Detail = p.Detail,
                    Feature = p.Feature,
                    Specifications = p.Specifications,
                    Helps = p.Helps,
                    IdVersion = p.IdVersion,
                    IsDeleted = p.IsDeleted,
                    Image = p.Image,
                    Category = p.Category
                }).ToList();

            Console.WriteLine($"Final Products: {products.Count}");

            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            return Ok(products);

        }

        // PUT: api/Products/SoftDelete1/{id}
        [HttpPut("SoftDelete1/{id}")]
        public async Task<IActionResult> SoftDeleteProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null || product.IsDeleted)
            {
                return NotFound("Category not found or it is already deleted.");
            }

            product.IsDeleted = true;

            try
            {
                // Cập nhật dữ liệu vào cơ sở dữ liệu
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // PUT: api/Products/UpdateDetails1
        [HttpPut("UpdateDetails1")]
        public async Task<IActionResult> UpdateProductDetails([FromQuery] string id, [FromBody] ProductUpdateRequest request)
        {
            // Tìm mục Product dựa trên ID
            var product = await _context.Products.FindAsync(id);

            if (product == null || product.IsDeleted)
            {
                return NotFound("Product not found or it is deleted.");
            }

            // Cập nhật CategoryName và Detail
            product.Name = request.Name;
            product.IdCate = request.IdCate;
            product.Image = request.Image;
            product.SL = request.SL;
            product.Price = request.Price;
            product.Detail = request.Detail;
            product.Feature = request.Feature;
            product.Specifications = request.Specifications;
            product.Helps = request.Helps;
            product.IdVersion = request.IdVersion;

            try
            {
                // Cập nhật dữ liệu vào cơ sở dữ liệu
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products/Insert1
        [HttpPost("Insert1")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductInsertRequest request)
        {
            // Kiểm tra xem ID đã tồn tại hay chưa
            if (ProductExists(request.ID))
            {
                return BadRequest("Product with the same ID already exists.");
            }

            // Tạo một đối tượng Product mới với dữ liệu từ request
            var newProduct = new Product
            {
                ID = request.ID,
                Name = request.Name,
                IdCate = request.IdCate,
                Evaluate = 0,
                SL = request.SL,
                Price = request.Price,
                Detail = request.Detail,
                Feature = request.Feature,
                Specifications = request.Specifications,
                Helps = request.Helps,
                IdVersion = request.IdVersion,
                IsDeleted = false,
                Image = request.Image
        };

            try
            {
                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi khi cập nhật cơ sở dữ liệu
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetProducts), new { id = newProduct.ID }, newProduct);
        }

        // Phương thức kiểm tra sự tồn tại của Product
        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.ID == id);
        }


    }
}
