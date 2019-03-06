using Microsoft.EntityFrameworkCore;
using TB.AspNetCore.Domain.Entitys;
using TB.AspNetCore.Infrastructrue.Config;

namespace TB.AspNetCore.Infrastructrue.Contexts
{
    public partial class RestApiContext : DbContext
    {
        public RestApiContext()
        {
        }

        public RestApiContext(DbContextOptions<RestApiContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<BackstageUser> BackstageUser { get; set; }
        public virtual DbSet<SystemActions> SystemActions { get; set; }
        public virtual DbSet<SystemRolePermission> SystemRolePermission { get; set; }
        public virtual DbSet<SystemRoles> SystemRoles { get; set; }
        public virtual DbSet<TaskSchedule> TaskSchedule { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var sqlStr= ConfigLocator.Instance.Get<string>("MySqlDbConnection");
                optionsBuilder.UseMySql(sqlStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
