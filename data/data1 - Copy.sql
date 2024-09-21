dùng câu lệnh của Mysql tạo theo các yêu cầu sau:

MembershipType
ID: Integer tự động tăng not null khóa chính
Type: String
IsDeleted: Boolean mặc định 0

Customer
ID: Integer tự động tăng not null khóa chính
CustomerName: String
BirthDate Date
Email: String
Phone: String
Image: String 
Gender: Boolean
Address: String
IsDeleted: Boolean mặc định 0

Member
ID: Integer tự động tăng not null khóa chính
Account: String not null unique
PassWord: String
IdCus: Integer là khóa ngoại tham chiếu tới ID của Customer
IdType: Integer là khóa ngoại tham chiếu tới ID của MembershipType
IsDeleted: Boolean mặc định 0

Log
ID: Integer tự động tăng not null khóa chính
IdMember: Integer là khóa ngoại tham chiếu tới ID của Member
Activity Date: Date
Detail: String 


Cart
ID: Integer tự động tăng not null khóa chính
IdMember: Integer là khóa ngoại tham chiếu tới ID của Member
SLTong: Integer mặc định 0
TongTien: Double

Category
ID: String not null khóa chính
CategoryName: String
Detail: String
IsDeleted: Boolean mặc định 0

ActivationCode
ID: Integer tự động tăng not null khóa chính
ActiCode: String giới hạn 12
ActiCodeNew: String giới hạn 12
NgayKhoiTao: Date
NgayHetHan: Date
DinhKy: Integer
IdCate: String  là khóa ngoại tham chiếu tới ID của Category
Price: Double

Product
ID: String not null khóa chính
ProductName: String
ldCate: String là khóa ngoại tham chiếu tới ID của Category
ldCode: Integer là khóa ngoại tham chiếu tới ID của ActivationCode
Evaluate: Double không được >5 và ko dc < 0
image: String
SL: Integer
Price: Double
Detail: String
Feature: String
Specifications: String
>Helps: String
Sitecode: String giới hạn 12 kí tự
MID: String giới hạn 12 kí tự
IsDeleted: Boolean mặc định 0

DetailCart
ID: Integer tự động tăng not null khóa chính
IdSP: String là khóa ngoại tham chiếu tới ID của Product
SiinCart Integer
DanhDaulnCart: Boolean mặc định 0

Orders
ID: Integer tự động tăng not null khóa chính
IdCart: Integer  là khóa ngoại tham chiếu tới ID của Cart
IdCus: Integer là khóa ngoại tham chiếu tới ID của Customer
DCGH: String
NgayDat: Date
NgayGiao: Date
PayMent: String
Thanh ToanStatus: String
DonHang Status: String