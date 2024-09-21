using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Model;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => !c.IsDeleted) // Lọc các mục không bị xóa
                .ToListAsync();

            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found.");
            }

            return Ok(categories);
        }

        // PUT: api/Categories/UpdateDetails
        [HttpPut("UpdateDetails")]
        public async Task<IActionResult> UpdateCategoryDetails([FromQuery] string id, [FromBody] CategoryUpdateRequest request)
        {
            // Tìm mục Category dựa trên ID
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted)
            {
                return NotFound("Category not found or it is deleted.");
            }

            // Cập nhật CategoryName và Detail
            category.CategoryName = request.CategoryName;
            category.Detail = request.Detail;

            try
            {
                // Cập nhật dữ liệu vào cơ sở dữ liệu
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // PUT: api/Categories/SoftDelete/{id}
        [HttpPut("SoftDelete/{id}")]
        public async Task<IActionResult> SoftDeleteCategory(string id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted)
            {
                return NotFound("Category not found or it is already deleted.");
            }

            // Đánh dấu là đã xóa
            category.IsDeleted = true;

            try
            {
                // Cập nhật dữ liệu vào cơ sở dữ liệu
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories/Insert
        [HttpPost("Insert")]
        public async Task<IActionResult> InsertCategory([FromBody] CategoryInsertRequest request)
        {
            // Kiểm tra xem ID đã tồn tại hay chưa
            if (CategoryExists(request.ID))
            {
                return BadRequest("Category with the same ID already exists.");
            }

            // Tạo một đối tượng Category mới với dữ liệu từ request
            var newCategory = new Category
            {
                ID = request.ID,
                CategoryName = request.CategoryName,
                Detail = request.Detail,
                IsDeleted = false  // Mới tạo nên không bị xóa
            };

            try
            {
                // Thêm vào cơ sở dữ liệu
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Xử lý lỗi khi cập nhật cơ sở dữ liệu
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetCategories), new { id = newCategory.ID }, newCategory);
        }


        // Phương thức kiểm tra sự tồn tại của Category
        private bool CategoryExists(string id)
        {
            return _context.Categories.Any(e => e.ID == id);
        }
    }
}
