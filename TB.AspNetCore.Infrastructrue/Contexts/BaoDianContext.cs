using Microsoft.EntityFrameworkCore;
using TB.AspNetCore.Domain.Entitys;

namespace TB.AspNetCore.Infrastructrue.Contexts
{
    public partial class BaoDianContext : DbContext
    {
        public BaoDianContext()
        {
        }

        public BaoDianContext(DbContextOptions<BaoDianContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Advertising> Advertising { get; set; }
        public virtual DbSet<BackstageUser> BackstageUser { get; set; }
        public virtual DbSet<BudgetInfo> BudgetInfo { get; set; }
        public virtual DbSet<Informations> Informations { get; set; }
        public virtual DbSet<MsgContent> MsgContent { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<ShareLookRecord> ShareLookRecord { get; set; }
        public virtual DbSet<SmsInfo> SmsInfo { get; set; }
        public virtual DbSet<SystemActions> SystemActions { get; set; }
        public virtual DbSet<SystemRolePermission> SystemRolePermission { get; set; }
        public virtual DbSet<SystemRoles> SystemRoles { get; set; }
        public virtual DbSet<SystemSetting> SystemSetting { get; set; }
        public virtual DbSet<TaskSchedule> TaskSchedule { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=35.240.208.158;Database=BaoDian;User=root;Password=YAya123...");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnType("varchar(36)");

                entity.Property(e => e.AccountStatus)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AccountType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Email).HasColumnType("varchar(36)");

                entity.Property(e => e.HeaderPic)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.LastLoginTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.NicikName)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.OpenId).HasColumnType("varchar(255)");

                entity.Property(e => e.PassWord)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.RealName)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ReferId).HasColumnType("varchar(36)");

                entity.Property(e => e.RewarCounts)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TelPhone).HasColumnType("varchar(20)");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Unionid).HasColumnType("varchar(50)");

                entity.Property(e => e.WxNick).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Advertising>(entity =>
            {
                entity.ToTable("advertising");

                entity.Property(e => e.Id).HasColumnType("varchar(36)");

                entity.Property(e => e.AdDesc).HasColumnType("text");

                entity.Property(e => e.AdLink).HasColumnType("varchar(255)");

                entity.Property(e => e.AdLocation)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AdName)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.AdPic).HasColumnType("text");

                entity.Property(e => e.BeginTime).HasColumnType("datetime");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.IsEnable)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");
            });

            modelBuilder.Entity<BackstageUser>(entity =>
            {
                entity.ToTable("backstage_user");

                entity.HasIndex(e => e.RoleId)
                    .HasName("rId");

                entity.Property(e => e.Id)
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.AccountStatus)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.AccountType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.FullName)
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Gender)
                    .HasColumnType("int(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IdCard)
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.LastLoginTime).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("'123456'");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SourceType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.BackstageUser)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("rId");
            });

            modelBuilder.Entity<BudgetInfo>(entity =>
            {
                entity.ToTable("budget_info");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AccountId).HasColumnType("varchar(36)");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("'0.00'");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.OrderId).HasColumnType("varchar(36)");

                entity.Property(e => e.Type)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Informations>(entity =>
            {
                entity.ToTable("informations");

                entity.Property(e => e.Id).HasColumnType("varchar(36)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Picture)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<MsgContent>(entity =>
            {
                entity.ToTable("msg_content");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.AreaType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.ContextType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.IsHasOutLink)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Latiude).HasColumnType("decimal(10,2)");

                entity.Property(e => e.Longtude).HasColumnType("decimal(10,2)");

                entity.Property(e => e.OutLink).HasColumnType("varchar(255)");

                entity.Property(e => e.Pics).HasColumnType("text");

                entity.Property(e => e.RemainCounts).HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.TotalCounts).HasColumnType("int(11)");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.Id).HasColumnType("varchar(36)");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ActualAmount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.AlipayId).HasColumnType("varchar(36)");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10,0)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.OrderCode).HasColumnType("varchar(36)");

                entity.Property(e => e.OrderStatus)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.OrderType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PayAmount).HasColumnType("decimal(10,2)");

                entity.Property(e => e.PayType)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.SourceId).HasColumnType("varchar(36)");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ShareLookRecord>(entity =>
            {
                entity.ToTable("share_look_record");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Cid)
                    .HasColumnName("CId")
                    .HasColumnType("bigint(20)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Type)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<SmsInfo>(entity =>
            {
                entity.ToTable("sms_info");

                entity.Property(e => e.Id).HasColumnType("bigint(20)");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Code).HasColumnType("varchar(20)");

                entity.Property(e => e.Contents).HasColumnType("varchar(50)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.IsUsed)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'b\\'0\\''");

                entity.Property(e => e.Mobile).HasColumnType("varchar(20)");
            });

            modelBuilder.Entity<SystemActions>(entity =>
            {
                entity.HasKey(e => e.ActionId);

                entity.ToTable("system_actions");

                entity.Property(e => e.ActionId)
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ActionDescription)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Orders).HasColumnType("int(11)");

                entity.Property(e => e.ParentAction).HasColumnType("varchar(255)");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<SystemRolePermission>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.ActionId });

                entity.ToTable("system_role_permission");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ActionId).HasColumnType("varchar(36)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });

            modelBuilder.Entity<SystemRoles>(entity =>
            {
                entity.ToTable("system_roles");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.ToTable("system_setting");

                entity.HasIndex(e => e.Name)
                    .HasName("uniqueName")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("varchar(36)");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.Value).HasColumnType("text");
            });

            modelBuilder.Entity<TaskSchedule>(entity =>
            {
                entity.ToTable("task_schedule");

                entity.Property(e => e.Id)
                    .HasColumnType("varchar(36)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CreateAuthr)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.CronExpress).HasColumnType("varchar(50)");

                entity.Property(e => e.EndRunTime).HasColumnType("datetime");

                entity.Property(e => e.JobGroup)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.NextRunTime).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasColumnType("text");

                entity.Property(e => e.RunStatus)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.StarRunTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");
            });
        }
    }
}
