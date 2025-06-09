using DB_BankProject.ModelsDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DB_BankProject.ModelsDB;

public partial class BankProjectContext : DbContext {
    public static string? StrConn { get; private set; }
    public BankProjectContext() { }

    public BankProjectContext(DbContextOptions<BankProjectContext> options)
        : base(options)
    { }
    public virtual DbSet<AdminAccount> AdminAccounts { get; set; }

    public virtual DbSet<Cdt> Cdts { get; set; }
    public virtual DbSet<Tran> Trans { get; set; }
    public virtual DbSet<TransHistory> TransHistories { get; set; }
    public virtual DbSet<UserAccount> UserAccounts { get; set; }
    public virtual DbSet<UserInfo> UserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        if (!optionsBuilder.IsConfigured){
            var config = new ConfigurationBuilder()
                             .SetBasePath(AppContext.BaseDirectory)
                             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .Build();

            StrConn = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(StrConn);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<AdminAccount>(entity => {
            entity.HasKey(e => e.AdminId).HasName("PK__AdminAcc__AD0500A625E2B30E");

            entity.ToTable("AdminAccount");

            entity.HasIndex(e => e.Number, "UQ__AdminAcc__FD291E412CFAA07C").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("adminId");
            entity.Property(e => e.HashPassword)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("hashPassword");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("number");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("ownerName");
        });

        modelBuilder.Entity<Cdt>(entity => {
            entity.HasKey(e => e.CdtId).HasName("PK__CDT__3B296C66D7436261");

            entity.ToTable("CDT");

            entity.Property(e => e.CdtId).HasColumnName("cdtId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Profit)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("profit");
            entity.Property(e => e.CdtStatus).HasColumnName("cdtStatus");
            entity.Property(e => e.DurationMonths).HasColumnName("durationMonths");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.InterestRate)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("interestRate");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Cdts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CDT_UserAccount");
        });

        modelBuilder.Entity<Tran>(entity => {
            entity.HasKey(e => e.TransId).HasName("PK__Trans__DB107FA7BEFB533D");

            entity.Property(e => e.TransId).HasColumnName("transId");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.DateTrans)
                .HasColumnType("datetime")
                .HasColumnName("dateTrans");
            entity.Property(e => e.ReceiverId).HasColumnName("receiverId");
            entity.Property(e => e.SenderId).HasColumnName("senderId");
            entity.Property(e => e.TransType).HasColumnName("transType");
            entity.Property(e => e.BranchId).HasColumnName("branchId");

            entity.HasOne(d => d.Receiver).WithMany(p => p.TranReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("FK_Trans_UserAccount_Receiver");

            entity.HasOne(d => d.Sender).WithMany(p => p.TranSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Trans_UserAccount_Sender");
        });

        modelBuilder.Entity<TransHistory>(entity => {
            entity.HasKey(e => e.HistId).HasName("PK__TransHis__4C5C5B844DBCC62A");

            entity.ToTable("TransHistory");

            entity.Property(e => e.HistId).HasColumnName("histId");
            entity.Property(e => e.TransId).HasColumnName("transId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Trans).WithMany(p => p.TransHistories)
                .HasForeignKey(d => d.TransId)
                .HasConstraintName("FK_TransHistory_Trans");

            entity.HasOne(d => d.User).WithMany(p => p.TransHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TransHistory_UserAccount");
        });

        modelBuilder.Entity<UserAccount>(entity => {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__CB9A1CFF1D1B4A36");

            entity.ToTable("UserAccount", tb =>
            {
                tb.HasTrigger("UserAccount_AD");
                tb.HasTrigger("UserAccount_AI");
                tb.HasTrigger("UserAccount_AU");
            });

            entity.HasIndex(e => e.Number, "UQ__UserAcco__FD291E41052FAB04").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Funds)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("funds");
            entity.Property(e => e.InvestedMoney)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("investedMoney");
            entity.Property(e => e.HashPassword)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("hashPassword");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("number");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("ownerName");
            entity.Property(e => e.RegisteredAt)
                .HasColumnType("datetime")
                .HasColumnName("registeredAt");
        });

        modelBuilder.Entity<UserInfo>(entity => {
            entity.HasKey(e => e.Id).HasName("PK__UserInfo__3213E83FDC087C84");

            entity.ToTable("UserInfo");

            entity.HasIndex(e => e.Number, "UQ__UserInfo__FD291E41A85F029D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("number");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(24)
                .IsUnicode(false)
                .HasColumnName("ownerName");
            entity.Property(e => e.RegisteredAt)
                .HasColumnType("datetime")
                .HasColumnName("registeredAt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}