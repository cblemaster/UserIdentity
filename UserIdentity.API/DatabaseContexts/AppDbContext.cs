using Microsoft.EntityFrameworkCore;
using UserIdentity.Core;
using UserIdentity.Core.Entities;

namespace UserIdentity.API.DatabaseContexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.Role1, "UC_Role_Role").IsUnique();

            entity.Property(e => e.Role1)
                .HasMaxLength(DataSchemaConstants.ROLE_ROLE_MAX_LENGTH)
                .IsUnicode(false)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UC_User_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UC_User_Phone").IsUnique();

            entity.HasIndex(e => e.Username, "UC_User_Username").IsUnique();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(DataSchemaConstants.USER_EMAIL_MAX_LENGTH)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(DataSchemaConstants.USER_FIRSTNAME_MAX_LENGTH)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(DataSchemaConstants.USER_LASTNAME_MAX_LENGTH)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(DataSchemaConstants.USER_PASSWORDHASH_MAX_LENGTH)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(DataSchemaConstants.USER_PHONE_MAX_LENGTH)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Salt)
                .HasMaxLength(DataSchemaConstants.USER_SALT_MAX_LENGTH)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(DataSchemaConstants.USER_USERNAME_MAX_LENGTH)
                .IsUnicode(false);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleUser",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RoleUser_Role"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RoleUser_User"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("RoleUser");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
