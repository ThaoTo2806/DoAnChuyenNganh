using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Net.Mail;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _cartDirectory = @"D:\HOCTAP\HK7\DoAn1Chi\DoAnChuyenNganh\data"; 
        private readonly string _usersDirectory = @"D:\HOCTAP\HK7\DoAn1Chi\DoAnChuyenNganh\data";

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequestModel loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Account) || string.IsNullOrEmpty(loginRequest.PassWord))
            {
                return BadRequest("Account and Password are required");
            }

            var user = await _context.Users
                .Where(u => u.Account == loginRequest.Account && u.PassWord == loginRequest.PassWord && !(u.IsDeleted ?? false)) // Check if IsDeleted is false
                .Select(u => new User
                {
                    IdUser = u.IdUser,
                    Account = u.Account,
                    PassWord = u.PassWord,
                    Email = u.Email,
                    Phone = u.Phone,
                    Address = u.Address,
                    Gender = u.Gender ?? false,
                    Birth = u.Birth,
                    IdRole = u.IdRole ?? 2,
                    IsDeleted = u.IsDeleted ?? false
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Invalid credentials or the user is deleted");
            }

            if (user.IdRole == 1)
            {
                // Ghi log khi người dùng đăng nhập thành công
                await CreateLog(user.IdUser);
                return Ok(user); // Đăng nhập thành công
            }
            else
            {
                return Unauthorized("User is not authorized");
            }
        }

        [HttpGet("logs")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogsByMember()
        {
            try
            {
                var logs = await _context.Logs
                    .Where(l => l.IdMember == 1)
                    .OrderByDescending(l => l.ID)
                    .Select(l => new
                    {
                        l.ID,
                        l.Activity,
                        l.Detail
                    })
                    .ToListAsync();

                if (logs == null || !logs.Any())
                {
                    return NotFound("No logs found for the specified member.");
                }

                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Invalid user data.");
        //    }

        //    var user = new User
        //    {
        //        Account = request.Account,
        //        PassWord = request.PassWord,
        //        Username = request.Name,
        //        Phone = request.Phone,
        //        Email = request.Email
        //    };

        //    try
        //    {
        //        _context.Users.Add(user);
        //        await _context.SaveChangesAsync();

        //        return Ok(new { message = "User registered successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        //    }
        //}

        private async Task CreateLog(int userId)
        {
            var log = new Log
            {
                IdMember = userId,
                Activity = DateTime.Now, // Ngày hiện tại trên hệ thống
                Detail = "Login to System"
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }


        //// POST: api/Users/login-regular
        //[HttpPost("login-regular")]
        //public async Task<ActionResult<User>> LoginForRegularUser([FromBody] LoginRequestModel loginRequest)
        //{
        //    if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Account) || string.IsNullOrEmpty(loginRequest.PassWord))
        //    {
        //        return BadRequest("Account and Password are required");
        //    }

        //    var user = await _context.Users
        //        .Where(u => u.Account == loginRequest.Account && u.PassWord == loginRequest.PassWord && !(u.IsDeleted ?? false)) // Check if IsDeleted is false
        //        .Select(u => new User
        //        {
        //            ID = u.ID,
        //            Account = u.Account,
        //            PassWord = u.PassWord,
        //            IdType = u.IdType ?? false,
        //            Name = u.Name,
        //            Phone = u.Phone,
        //            Image = u.Image,
        //            Gender = u.Gender ?? false,
        //            Address = u.Address,
        //            IsDeleted = u.IsDeleted ?? false,
        //            Email = u.Email,
        //            Birth = u.Birth
        //        })
        //        .FirstOrDefaultAsync();

        //    if (user == null)
        //    {
        //        return Unauthorized("Invalid credentials or the user is deleted");
        //    }

        //    if (user.IdType == false)
        //    {
        //        await CreateLog(user.ID);

        //        // Save information to session
        //        SetUserSession(user);

        //        // Check if the XML file exists
        //        var userFromXml = LoadUserFromXml(user.ID);
        //        if (userFromXml == null)
        //        {
        //            // If the XML file does not exist, save user information to the XML file
        //            SaveUserToXml(user);
        //        }
        //        else
        //        {
        //            // If the XML file exists, update the cart from the XML file to the session
        //            var cart = LoadCartFromXml(user.ID);
        //            SetCartSession(cart);
        //        }

        //        return Ok(user); // Successful login
        //    }
        //    else
        //    {
        //        return Unauthorized("User is not authorized");
        //    }
        //}

        // GET: api/Users/profile
        [HttpGet("profile")]
        public ActionResult<User> GetProfile()
        {
            var userIdBase64 = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(userIdBase64))
            {
                return Unauthorized("User is not logged in");
            }

            // Log the base64 string for debugging
            Console.WriteLine($"UserID Base64: {userIdBase64}");

            try
            {
                var userIdBytes = Convert.FromBase64String(userIdBase64);
                var userIdString = Encoding.UTF8.GetString(userIdBytes);
                var userId = int.Parse(userIdString);

                // Log the decoded userId for debugging
                Console.WriteLine($"Decoded UserID: {userId}");

                var profile = LoadUserFromXml(userId);

                if (profile == null)
                {
                    return Unauthorized("User data not found");
                }

                return Ok(profile);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 format");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/Users/update-password
        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Account and Email are required");
            }

            try
            {
                var user = await _context.Users
                    .Where(u => u.Account == request.Account && u.Email == request.Email && !(u.IsDeleted ?? false))
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound("Account or Email does not match");
                }

                // Tạo mật khẩu ngẫu nhiên 12 ký tự
                var newPassword = GenerateRandomPassword();
                user.PassWord = newPassword;

                // Cập nhật người dùng trong cơ sở dữ liệu
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                await SendPasswordByEmail(request.Email, newPassword);

                return Ok("Password updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        //// PUT: api/Users/Update
        //[HttpPut("Update")]
        //public async Task<IActionResult> UpdateUserDetails([FromBody] UserUpdateRequest request)
        //{
        //    var userIdBase64 = HttpContext.Session.GetString("UserID");
        //    if (string.IsNullOrEmpty(userIdBase64))
        //    {
        //        return Unauthorized("User is not logged in");
        //    }

        //    var userIdBytes = Convert.FromBase64String(userIdBase64);
        //    var userIdString = Encoding.UTF8.GetString(userIdBytes);
        //    var userId = int.Parse(userIdString);


        //    var user = await _context.Users.FindAsync(userId);

        //    if (user == null || user.IsDeleted == true)
        //    {
        //        return NotFound("User not found or is deleted");
        //    }

        //    user.Name = request.Name;
        //    user.Email = request.Email;
        //    user.Address = request.Address;
        //    user.Phone = request.Phone;
        //    user.Birth = request.Birth;
        //    user.Gender = request.Gender;
        //    user.Image = request.Image;

        //    try
        //    {
        //        _context.Entry(user).State = EntityState.Modified;
        //        await _context.SaveChangesAsync();
        //        SaveUserToXml(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error: {ex.Message}");
        //    }

        //    return Ok("User updated successfully");
        //}

        [HttpGet("regular-customers")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetRegularCustomers()
        {
            var customers = await (from c in _context.Customers
                                   join u in _context.Users on c.IdUser equals u.IdUser
                                   where c.IsDeleted == false && u.IdRole == 2
                                   select new CustomerDto
                                   {
                                       IdCustomer = c.idCustomer,
                                       Name = c.Name,
                                       Image = c.Image,
                                       IsDeleted = c.IsDeleted,
                                       IdUser = u.IdUser,
                                       Birth = u.Birth,
                                       Gender = u.Gender,
                                       Email = u.Email,
                                       Phone = u.Phone,
                                       Address = u.Address
                                   })
                       .ToListAsync();

            if (customers == null || !customers.Any())
            {
                return NotFound("No regular customers found");
            }

            return Ok(customers);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] UserInsertRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                var password = GenerateRandomPassword();

                var user = new User
                {
                    Account = request.Account,
                    PassWord = password,
                    Phone = request.Phone,
                    Gender = request.Gender,
                    Address = request.Address,
                    Email = request.Email,
                    Birth = request.Birth,
                    IdRole = 2,
                    IsDeleted = false
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await SendPasswordByEmail(request.Email, password);

                var userId = _context.Users
                .Where(u => u.Account == request.Account)
                .Select(u => u.IdUser)
                .FirstOrDefault();

                var cus = new Customer
                {
                    Name = request.Name,
                    Image = request.Image,
                    IdUser = userId,
                    IsDeleted = false
                };

                _context.Customers.Add(cus);
                await _context.SaveChangesAsync();

                return Ok("User and customer created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //public int GetUserIdByAccount(string account)
        //{
        //    if (string.IsNullOrWhiteSpace(account))
        //    {
        //        throw new ArgumentException("Account must not be null or empty.", nameof(account));
        //    }

        //    var userId = _context.Users
        //        .Where(u => u.Account == account)
        //        .Select(u => u.IdUser)
        //        .FirstOrDefault();

        //    return userId;
        //}

        private string GenerateRandomPassword(int length = 12)
        {
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                var password = new char[length];
                for (int i = 0; i < length; i++)
                {
                    password[i] = chars[random.Next(chars.Length)];
                }
                return new string(password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating a random password: {ex.Message}");
                throw;
            }
        }

        private async Task SendPasswordByEmail(string email, string password)
        {
            try
            {
                var fromAddress = new MailAddress("thaonguyen28062003@gmail.com", "Siglaz");
                var toAddress = new MailAddress(email);
                const string subject = "Your New Account Password";
                string body = $"Your new password is: {password}";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("thaonguyen28062003@gmail.com", "gxnrirdwvacvxjkr")
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    await smtp.SendMailAsync(message);
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP error: {smtpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
                throw;
            }
        }

        // POST: api/Users/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Xóa cookie nếu cần
            HttpContext.Response.Cookies.Delete("Cart");

            return Ok("User logged out successfully");
        }

        //private void SaveUserToXml(User user)
        //{
        //    if (!Directory.Exists(_usersDirectory))
        //    {
        //        Directory.CreateDirectory(_usersDirectory);
        //    }

        //    var userXmlPath = Path.Combine(_usersDirectory, $"{user.ID}.xml");
        //    var xmlSerializer = new XmlSerializer(typeof(User));
        //    using (var writer = new StreamWriter(userXmlPath))
        //    {
        //        xmlSerializer.Serialize(writer, user);
        //    }
        //}

        private User LoadUserFromXml(int userId)
        {
            var userXmlPath = Path.Combine(_usersDirectory, $"{userId}.xml");
            if (System.IO.File.Exists(userXmlPath))
            {
                var xmlSerializer = new XmlSerializer(typeof(User));
                using (var reader = new StreamReader(userXmlPath))
                {
                    return (User)xmlSerializer.Deserialize(reader);
                }
            }
            return null;
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

        private void SetCartSession(Cart cart)
        {
            HttpContext.Session.SetString("Cart", System.Text.Json.JsonSerializer.Serialize(cart));
        }

        //private void SetUserSession(User user)
        //{
        //    var userIdBytes = Encoding.UTF8.GetBytes(user.ID.ToString());
        //    var userIdBase64 = Convert.ToBase64String(userIdBytes);
        //    HttpContext.Session.SetString("UserID", userIdBase64);
        //}

        private Cart GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            return cartJson != null ? System.Text.Json.JsonSerializer.Deserialize<Cart>(cartJson) : new Cart();
        }
    }
}