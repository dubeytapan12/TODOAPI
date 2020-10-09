using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TODO.Entities
{
    public partial class TODOContext : DbContext
    {
        public TODOContext()
        {
        }

        public TODOContext(DbContextOptions<TODOContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserClaims> UserClaims { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }

        public virtual DbSet<TODOTask> TodoTask { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaims>(entity =>
            {
                entity.HasKey(e => e.ClaimId)
                    .HasName("PK__userClai__01BDF9D3071D22A7");

                entity.ToTable("userClaims");

                entity.Property(e => e.ClaimId).HasColumnName("claimId");

                entity.Property(e => e.ClaimType)
                    .HasColumnName("claimType")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClaimValue).HasColumnName("claimValue");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__userClaim__userI__239E4DCF");
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.ToTable("userDetails");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FName)
                    .IsRequired()
                    .HasColumnName("fName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LName)
                    .HasColumnName("lName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TODOTask>(entity =>
            {
                entity.ToTable("TaskTodo");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasColumnName("TaskName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasColumnName("Category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnName("StartDate")
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnName("EndDate")
                    .IsUnicode(false);
                entity.Property(e => e.IsAutoClose)
                   .HasColumnName("IsAutoClose")
                   .IsUnicode(false);
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
