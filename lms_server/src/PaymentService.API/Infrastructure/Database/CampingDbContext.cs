using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Domain;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PaymentService.API.Infrastructure.Database;

public partial class CampingDbContext : DbContext
{

    public virtual DbSet<Payments> Payments { get; set; }

    public CampingDbContext(DbContextOptions<CampingDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.OrderId, "order_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaidAt)
                .HasColumnType("datetime")
                .HasColumnName("paid_at");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasComment("e.g., credit_card, paypal, cod")
                .HasColumnName("payment_method");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasComment("e.g., pending, completed, failed")
                .HasColumnName("status");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(255)
                .HasComment("from payment gateway")
                .HasColumnName("transaction_id");
        });
    }
}
