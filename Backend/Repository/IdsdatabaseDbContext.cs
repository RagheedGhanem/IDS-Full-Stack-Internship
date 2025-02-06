using System;
using System.Collections.Generic;
using IDS_App.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace IDS_App.Repository;

public partial class IdsdatabaseDbContext : DbContext
{
    public IdsdatabaseDbContext()
    {
    }

    public IdsdatabaseDbContext(DbContextOptions<IdsdatabaseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vote> Votes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-L0OOLDL\\SQLEXPRESS;Initial Catalog=IDS_dbms;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC275BA0A1EB");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC273E680833");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments).HasConstraintName("FK__Comments__PostID__7908F585");

            entity.HasOne(d => d.User).WithMany(p => p.Comments).HasConstraintName("FK__Comments__UserID__79FD19BE");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC27542A2854");

            entity.Property(e => e.IsRead).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Notificat__UserI__19AACF41");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posts__3214EC27B8C44119");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Posts__CategoryI__214BF109");

            entity.HasOne(d => d.User).WithMany(p => p.Posts).HasConstraintName("FK__Posts__UserID__2057CCD0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC274615CF1E");

            entity.Property(e => e.ReputationPoints).HasDefaultValue(0);
            entity.Property(e => e.Role).HasDefaultValue("User");
        });

        modelBuilder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Votes__3214EC276797F0B8");

            entity.HasOne(d => d.Post).WithMany(p => p.Votes).HasConstraintName("FK__Votes__PostID__7DCDAAA2");

            entity.HasOne(d => d.User).WithMany(p => p.Votes).HasConstraintName("FK__Votes__UserID__7EC1CEDB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
