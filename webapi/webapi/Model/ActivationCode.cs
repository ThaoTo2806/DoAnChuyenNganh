namespace webapi.Model
{
    public class ActivationCode
    {
        public int ID { get; set; } // Định danh mã kích hoạt
        public string Code { get; set; } // Mã kích hoạt
        public DateTime ExpiryDate { get; set; } // Ngày hết hạn của mã kích hoạt
        public bool IsUsed { get; set; } // Trạng thái sử dụng mã kích hoạt
    }

}
