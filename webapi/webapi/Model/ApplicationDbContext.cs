using Microsoft.EntityFrameworkCore;

namespace webapi.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }  // Thêm DbSet<Log> để sử dụng Log trong DbContext
        public DbSet<Category> Categories { get; set; } // Thêm DbSet<Category> cho bảng Category
        public DbSet<ActivationCode> ActivationCodes { get; set; } // Thêm DbSet cho ActivationCode
        public DbSet<Product> Products { get; set; } // Thêm DbSet cho Product
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order1> orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình cho bảng User
            modelBuilder.Entity<User>()
                .ToTable("User")
                .HasKey(u => u.ID);  // Đảm bảo rằng có một khóa chính

            modelBuilder.Entity<User>()
                .Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            // Cấu hình cho bảng Category
            modelBuilder.Entity<Category>()
                .ToTable("Category")
                .HasKey(c => c.ID);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            // Cấu hình cho bảng Log
            modelBuilder.Entity<Log>()
                .ToTable("Log")
                .HasKey(l => l.ID);

            modelBuilder.Entity<Log>()
                .HasOne(l => l.User)
                .WithMany() 
                .HasForeignKey(l => l.IdMember);

            modelBuilder.Entity<Log>()
                .Property(l => l.Activity)
                .HasDefaultValueSql("CURRENT_DATE");

            // Cấu hình bảng ActivationCode
            modelBuilder.Entity<ActivationCode>()
                .ToTable("ActivationCode")
                .HasKey(a => a.ID);

            // Cấu hình bảng Product
            modelBuilder.Entity<Product>()
                .ToTable("Product")
                .HasKey(u => u.ID);

            modelBuilder.Entity<Product>()
                .Property(c => c.IsDeleted)
                .HasDefaultValue(false);  // Đặt giá trị mặc định cho IsDeleted

            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.ldCate)
            .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
