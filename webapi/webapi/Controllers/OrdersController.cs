using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using webapi.Model;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using System.Text;
using webapi.Model;
using System.Net;
using System.Net.Mail;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _cartDirectory = @"D:\HOCTAP\HK7\DoAn1Chi\DoAnChuyenNganh\data";
        private readonly ILogger<OrdersController> _logger;
        private static readonly List<ActivationRequest> _activationRequests = new List<ActivationRequest>();


        public OrdersController(ApplicationDbContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
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
                else
                {
                    _logger.LogWarning($"Cart XML file does not exist for userId {userId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading cart from XML: {ex.Message}");
            }

            return new Cart();
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
                _logger.LogError(ex, $"Error saving cart to XML: {ex.Message}");
            }
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            int newOrderId = 0;
            int newCodeId = 0;
            string status = "";
            DateTime creationDate = DateTime.Now;
            DateTime expiryDate = DateTime.Now;
            DateTime updateDate = DateTime.Now;
            decimal price = 0;

            try
            {
                // Lấy UserID từ Session và decode từ Base64
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    _logger.LogWarning("User is not logged in");
                    return Unauthorized("User is not logged in");
                }

                string userIdDecoded;
                try
                {
                    userIdDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(userIdBase64));
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, "Invalid Base64 string: {UserIdBase64}", userIdBase64);
                    return BadRequest("Invalid Base64 string for UserID");
                }

                if (!int.TryParse(userIdDecoded, out int userId))
                {
                    _logger.LogError("UserID is not a valid integer: {UserIdDecoded}", userIdDecoded);
                    return BadRequest("UserID is not a valid integer");
                }

                // Load cart from XML file
                var cart = LoadCartFromXml(userId);
                if (cart.Items == null || !cart.Items.Any())
                {
                    return BadRequest("Cart is empty");
                }

                var totalQuantity = cart.TotalQuantity;
                var totalPrice = cart.TotalPrice + (totalQuantity * 89.99);

                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    // Gọi stored procedure CreateOrder
                    using (var command = new MySqlCommand("CreateOrder", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@p_IdUser", userId);
                        command.Parameters.AddWithValue("@p_DCGH", request.DeliveryAddress);
                        command.Parameters.AddWithValue("@p_ThanhToanStatus", request.PaymentStatus);
                        command.Parameters.AddWithValue("@p_TotalQuantity", totalQuantity);
                        command.Parameters.AddWithValue("@p_TotalPrice", (decimal)totalPrice);
                        command.Parameters.Add("@p_OrderId", MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        await command.ExecuteNonQueryAsync();

                        newOrderId = Convert.ToInt32(command.Parameters["@p_OrderId"].Value);

                        var productData = cart.Items.Select(item => new
                        {
                            IdProduct = item.ProductId,
                            Amount = item.Quantity,
                            priceCode = 89.99,
                            priceProduct = item.Price
                        }).ToArray();

                        // Gọi stored procedure CreateActiveCode
                        using (var createActiveCodeCommand = new MySqlCommand("CreateActiveCode", connection))
                        {
                            createActiveCodeCommand.CommandType = System.Data.CommandType.StoredProcedure;

                            // Tham số đầu vào cho stored procedure
                            int dinhKyValue = 6; // Thay đổi giá trị này theo nhu cầu (Change this value as needed)
                            createActiveCodeCommand.Parameters.AddWithValue("@p_DinhKy", dinhKyValue);

                            // Các tham số OUT cho stored procedure
                            createActiveCodeCommand.Parameters.AddWithValue("@p_ActiCodeID", MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                            createActiveCodeCommand.Parameters.AddWithValue("@p_ActiCode", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                            createActiveCodeCommand.Parameters.AddWithValue("@p_Status", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                            createActiveCodeCommand.Parameters.AddWithValue("@p_NgayKhoiTao", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                            createActiveCodeCommand.Parameters.AddWithValue("@p_NgayHetHan", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                            createActiveCodeCommand.Parameters.AddWithValue("@p_Price", DBNull.Value).Direction = System.Data.ParameterDirection.Output;

                            // Thực hiện stored procedure
                            await createActiveCodeCommand.ExecuteNonQueryAsync();

                            // Lấy kết quả từ các tham số OUT
                            newCodeId = Convert.ToInt32(createActiveCodeCommand.Parameters["@p_ActiCodeID"].Value);
                            status = createActiveCodeCommand.Parameters["@p_Status"].Value.ToString();
                            creationDate = Convert.ToDateTime(createActiveCodeCommand.Parameters["@p_NgayKhoiTao"].Value);
                            expiryDate = Convert.ToDateTime(createActiveCodeCommand.Parameters["@p_NgayHetHan"].Value);
                            price = Convert.ToDecimal(createActiveCodeCommand.Parameters["@p_Price"].Value);
                        }

                        var productDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(productData);

                        // Gọi stored procedure CreateOrderDetail
                        using (var detailCommand = new MySqlCommand("CreateOrderDetail", connection))
                        {
                            detailCommand.CommandType = System.Data.CommandType.StoredProcedure;

                            detailCommand.Parameters.AddWithValue("@p_OrderId", newOrderId);
                            detailCommand.Parameters.AddWithValue("@p_IdUser", userId);
                            detailCommand.Parameters.AddWithValue("@v_ActiCode", newCodeId);
                            detailCommand.Parameters.AddWithValue("@p_ProductData", productDataJson);

                            await detailCommand.ExecuteNonQueryAsync();
                        }


                    }
                }

                // Lưu cart vào XML
                SaveCartToXml(userId, new Cart());

                // Trả về thông tin đơn hàng và mã kích hoạt
                return Ok(new { OrderId = newOrderId, ActivationCodeId = newCodeId, Status = status, CreationDate = creationDate, ExpiryDate = expiryDate, UpdateDate = updateDate, Price = price });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("order-info")]
        public async Task<IActionResult> GetOrderInformation()
        {
            try
            {
                // Lấy UserID từ Session và decode từ Base64
                var userIdBase64 = HttpContext.Session.GetString("UserID");
                if (string.IsNullOrEmpty(userIdBase64))
                {
                    _logger.LogWarning("User is not logged in");
                    return Unauthorized("User is not logged in");
                }

                string userIdDecoded;
                try
                {
                    userIdDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(userIdBase64));
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, "Invalid Base64 string: {UserIdBase64}", userIdBase64);
                    return BadRequest("Invalid Base64 string for UserID");
                }

                if (!int.TryParse(userIdDecoded, out int userId))
                {
                    _logger.LogError("UserID is not a valid integer: {UserIdDecoded}", userIdDecoded);
                    return BadRequest("UserID is not a valid integer");
                }

                // Đọc thông tin người dùng từ file XML
                var userXmlPath = Path.Combine(_cartDirectory, $"{userId}.xml");
                if (!System.IO.File.Exists(userXmlPath))
                {
                    return NotFound("User information not found");
                }

                User_Customer user;
                try
                {
                    var xmlSerializer = new XmlSerializer(typeof(User_Customer));
                    using (var reader = new StreamReader(userXmlPath))
                    {
                        user = (User_Customer)xmlSerializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error loading user information from XML: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error loading user information from XML: {ex.Message}");
                }

                // Load cart từ file XML
                var cart = LoadCartFromXml(userId);
                if (cart.Items == null || !cart.Items.Any())
                {
                    return BadRequest("Cart is empty");
                }

                // Tính tổng giá giỏ hàng và cộng thêm giá của mã kích hoạt
                var activationCodePrice = 89.99; // Chuyển đổi thành double
                var totalPrice = cart.TotalPrice + (activationCodePrice * cart.TotalQuantity);

                var orderInformation = new OrderInformation
                {
                    Name = user.Name,
                    Phone = user.Phone,
                    Address = user.Address,
                    CartItems = cart.Items,
                    TotalPrice = totalPrice,
                    ActivationCodeInfo = $"Periodic: 6 months, Price: $89.99 x {cart.TotalQuantity}"
                };

                return Ok(orderInformation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order information");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("processing-orders")]
        public async Task<IActionResult> GetProcessingOrders()
        {
            try
            {
                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("GetProcessingOrders", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var orders = new List<object>();
                            while (await reader.ReadAsync())
                            {
                                var order = new
                                {
                                    OrderId = reader.GetInt32("ID"),
                                    IdUser = reader.GetInt32("IdUser"),
                                    DCGH = reader.GetString("DCGH"),
                                    NgayDat = reader.GetDateTime("NgayDat").ToString("yyyy-MM-dd"),
                                    DonHangStatus = reader.GetString("DonHangStatus"),
                                    TotalQuantity = reader.GetInt32("SLTong"),
                                    TotalPrice = reader.GetDecimal("TongTien")
                                };
                                orders.Add(order);
                            }

                            return Ok(orders);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching processing orders");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("confirmed-delivered-orders")]
        public async Task<IActionResult> GetConfirmedOrDeliveredOrders()
        {
            try
            {
                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("GetConfirmedOrDeliveredOrders", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var orders = new List<object>();
                            while (await reader.ReadAsync())
                            {
                                var order = new
                                {
                                    OrderId = reader.GetInt32("ID"),
                                    IdUser = reader.GetInt32("IdUser"),
                                    DCGH = reader.GetString("DCGH"),
                                    NgayDat = reader.GetDateTime("NgayDat").ToString("yyyy-MM-dd"),
                                    NgayGiao = reader.IsDBNull(reader.GetOrdinal("NgayGiao")) ? (string)null : reader.GetDateTime("NgayGiao").ToString("yyyy-MM-dd"),
                                    ThanhToanStatus = reader.GetString("ThanhToanStatus"),
                                    DonHangStatus = reader.GetString("DonHangStatus"),
                                    TotalQuantity = reader.GetInt32("SLTong"),
                                    TotalPrice = reader.GetDecimal("TongTien")
                                };
                                orders.Add(order);
                            }

                            return Ok(orders);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching processing orders");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("order-details/{orderId}")]
        public async Task<IActionResult> GetOrderDetailsById(int orderId)
        {
            try
            {
                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("GetOrderDetailsById", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@in_order_id", orderId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var orderDetail = new OrderDetailModel
                                {
                                    OrderID = reader.GetInt32("OrderID"),
                                    AccCus = reader.IsDBNull(reader.GetOrdinal("AccCus")) ? null : reader.GetString("AccCus"),
                                    UserName = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString("Name"),
                                    UserEmail = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email"),
                                    UserPhone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
                                    UserAddress = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString("Address"),
                                    Products = reader.IsDBNull(reader.GetOrdinal("Products")) ? null : reader.GetString("Products"),
                                    SLSP = reader.IsDBNull(reader.GetOrdinal("SLSP")) ? null : reader.GetString("SLSP"),
                                    GSP = reader.IsDBNull(reader.GetOrdinal("GSP")) ? null : reader.GetString("GSP"),
                                    Versions = reader.IsDBNull(reader.GetOrdinal("Versions")) ? null : reader.GetString("Versions"),
                                    TotalVersionPrice = reader.IsDBNull(reader.GetOrdinal("TotalVersionPrice")) ? 0 : reader.GetDecimal("TotalVersionPrice"),
                                    TotalPrice = reader.IsDBNull(reader.GetOrdinal("TongTien")) ? 0 : reader.GetDecimal("TongTien"),
                                    OrderStatus = reader.IsDBNull(reader.GetOrdinal("DonHangStatus")) ? null : reader.GetString("DonHangStatus")
                                };

                                return Ok(orderDetail);
                            }
                            else
                            {
                                return NotFound("Order not found or has no details.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order details");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("UpdateOrders")]
        public async Task<IActionResult> UpdateOrderDetails([FromQuery] int id, [FromBody] OrderUpdateRequest request)
        {
            var order = await _context.orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Cập nhật Status
            order.DonHangStatus = request.Status;
            order.note = request.note;

            try
            {
                // Cập nhật dữ liệu vào cơ sở dữ liệu
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
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

                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("GetOrderDetailsByUserId", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@userId", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var orders = new List<object>();
                            while (await reader.ReadAsync())
                            {
                                var order = new
                                {
                                    OrderID = reader.GetInt32("OrderID"),
                                    FirstImage = reader.GetString("FirstImage"),
                                    FirstProductName = reader.GetString("FirstProductName"),
                                    OrderDate = reader.GetDateTime("OrderDate"),
                                    DeliveryDate = reader.GetDateTime("DeliveryDate"),
                                    OrderStatus = reader.GetString("OrderStatus")
                                };
                                orders.Add(order);
                            }

                            return Ok(orders);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("userorder-details")]
        public async Task<IActionResult> GetOrderDetails([FromQuery] int orderId)
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

                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("GetOrderDetails", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("idorder", orderId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var orderDetails = new UserOrderDetails
                            {
                                Order = null,
                                Products = new List<ProductDetails>()
                            };

                            while (await reader.ReadAsync())
                            {
                                if (orderDetails.Order == null)
                                {
                                    orderDetails.Order = new OrderDetails
                                    {
                                        OrderId = reader.GetInt32("IdOrder"),
                                        UserName = reader.GetString("Name"),
                                        Phone = reader.GetString("Phone"),
                                        DeliveryAddress = reader.GetString("DCGH"),
                                        OrderDate = reader.GetDateTime("NgayDat"),
                                        DeliveryDate = reader.GetDateTime("NgayGiao"),
                                        TotalQuantity = reader.GetInt32("SLTong"),
                                        TotalPrice = reader.GetDecimal("TongTien"),
                                        PaymentStatus = reader.GetString("ThanhToanStatus"),
                                        OrderStatus = reader.GetString("DonHangStatus"),
                                        ActiCode = reader.IsDBNull(reader.GetOrdinal("ActiCode")) ? null : reader.GetString("ActiCode"),
                                        DinhKy = reader.IsDBNull(reader.GetOrdinal("DinhKy")) ? null : reader.GetString("DinhKy"),
                                        ActivationPrice = reader.IsDBNull(reader.GetOrdinal("ActivationPrice")) ? (decimal?)null : reader.GetDecimal("ActivationPrice"),
                                        NgayKhoiTao = reader.IsDBNull(reader.GetOrdinal("NgayKhoiTao")) ? (DateTime?)null : reader.GetDateTime("NgayKhoiTao"),
                                        NgayHetHan = reader.IsDBNull(reader.GetOrdinal("NgayHetHan")) ? (DateTime?)null : reader.GetDateTime("NgayHetHan")
                                    };
                                }

                                orderDetails.Products.Add(new ProductDetails
                                {
                                    ProductImage = reader.GetString("ProductImage"),
                                    ProductName = reader.GetString("ProductName"),
                                    ProductVersion = reader.GetString("ProductVersion"),
                                    Quantity = reader.GetInt32("Amount"),
                                    ProductPrice = reader.GetDouble("ProductPrice"),
                                    VersionPrice = reader.GetDouble("VersionPrice")
                                });
                            }

                            if (orderDetails.Order != null && orderDetails.Products.Any())
                            {
                                return Ok(orderDetails);
                            }
                            else
                            {
                                return NotFound("Order not found or has no details.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order details");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("expired-code-orders")]
        public async Task<IActionResult> GetExpiredCodeOrders()
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

                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("GetExpiredCodeByUserId", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@userId", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var orders = new List<object>();
                            while (await reader.ReadAsync())
                            {
                                var order = new
                                {
                                    OrderID = reader.GetInt32("OrderID"),
                                    FirstImage = reader.GetString("FirstImage"),
                                    FirstProductName = reader.GetString("FirstProductName"),
                                    FirstVersion = reader.GetString("FirstVersion"),
                                    InitiatedDate = reader.GetDateTime("InitiatedDate"),
                                    ExpiredDate = reader.GetDateTime("ExpiredDate"),
                                    CStatus = reader.GetString("CStatus")
                                };
                                orders.Add(order);
                            }

                            return Ok(orders);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("request-activation")]
        public IActionResult RequestActivation([FromBody] ActivationRequest request)
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

                if (request == null)
                {
                    return BadRequest("Invalid request data");
                }

                _activationRequests.Add(request);

                return Ok("Activation request added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("activation-requests")]
        public IActionResult GetActivationRequests()
        {
            try
            {
                return Ok(_activationRequests);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("update-activationcode")]
        public async Task<IActionResult> UpdateActiveCode([FromBody] UpdateActiceCode request)
        {
            int newCodeId = 0;
            string newActiveCode = "";

            try
            {
                // Kết nối đến MySQL
                using (var connection = new MySqlConnection(_context.Database.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    // Gọi stored procedure CreateActiveCode
                    using (var createActiveCodeCommand = new MySqlCommand("CreateActiveCode", connection))
                    {
                        createActiveCodeCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        // Tham số đầu vào cho stored procedure
                        int dinhKyValue = request.dinhKy;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_DinhKy", dinhKyValue);

                        // Các tham số OUT cho stored procedure
                        createActiveCodeCommand.Parameters.AddWithValue("@p_ActiCodeID", MySqlDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_ActiCode", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_Status", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_NgayKhoiTao", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_NgayHetHan", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_NgayCapNhat", DBNull.Value).Direction = System.Data.ParameterDirection.Output;
                        createActiveCodeCommand.Parameters.AddWithValue("@p_Price", DBNull.Value).Direction = System.Data.ParameterDirection.Output;

                        // Thực hiện stored procedure
                        await createActiveCodeCommand.ExecuteNonQueryAsync();

                        // Lấy kết quả từ các tham số OUT
                        newCodeId = Convert.ToInt32(createActiveCodeCommand.Parameters["@p_ActiCodeID"].Value);
                        newActiveCode = createActiveCodeCommand.Parameters["@p_ActiCode"].Value.ToString();
                    }

                    // Update OrderDetail and pro_acc tables
                    using (var updateOrderDetailCommand = new MySqlCommand("UPDATE OrderDetail SET IdActiveCode = @newCodeId WHERE IdOrder = @idOrder", connection))
                    {
                        updateOrderDetailCommand.Parameters.AddWithValue("@newCodeId", newCodeId);
                        updateOrderDetailCommand.Parameters.AddWithValue("@idOrder", request.IdOrder);

                        await updateOrderDetailCommand.ExecuteNonQueryAsync();
                    }

                    using (var updateProAccCommand = new MySqlCommand("UPDATE pro_acc SET idMKH = @newCodeId WHERE idOrder = @idOrder", connection))
                    {
                        updateProAccCommand.Parameters.AddWithValue("@newCodeId", newCodeId);
                        updateProAccCommand.Parameters.AddWithValue("@idOrder", request.IdOrder);

                        await updateProAccCommand.ExecuteNonQueryAsync();
                    }

                    // Xóa yêu cầu khỏi danh sách _activationRequests
                    var activationRequest = _activationRequests.FirstOrDefault(r => r.OrderID == request.IdOrder);
                    if (activationRequest != null)
                    {
                        _activationRequests.Remove(activationRequest);
                    }
                }

                await CreateLog();
                await SendPasswordByEmail(request.Email, newActiveCode);
                return Ok("Activation code generated successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        private async Task CreateLog()
        {
            var log = new Log
            {
                IdMember = 1,
                Activity = DateTime.Now,
                Detail = "Generate Activation Code"
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        private async Task SendPasswordByEmail(string email, string newCode)
        {
            try
            {
                var fromAddress = new MailAddress("thaonguyen28062003@gmail.com", "Siglaz");
                var toAddress = new MailAddress(email);
                const string subject = "Respond To A Request To Generate Activation Code";
                string body = $"The new activation code for your product is:: {newCode}";

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

    }
}
