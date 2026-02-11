using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Acceloka.Entities;

public partial class AccelokaContext : DbContext
{
    public AccelokaContext()
    {
    }

    public AccelokaContext(DbContextOptions<AccelokaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookedTicket> BookedTickets { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Initial Catalog=ACCELOKA;User Id=calvin;pwd=calvin; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookedTicket>(entity =>
        {
            entity.ToTable("BookedTicket");

            entity.Property(e => e.BookedTicketId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("SYSTEM");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseDate).HasDefaultValueSql("(sysdatetimeoffset())", "DF_BookedTicket_PurchaseDate");
            entity.Property(e => e.ScheduledDate).HasDefaultValueSql("(sysdatetimeoffset())", "DF_BookedTicket_ScheduledDate");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("SYSTEM");

            entity.HasOne(d => d.TicketCodeNavigation).WithMany(p => p.BookedTickets)
                .HasForeignKey(d => d.TicketCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookedTicket_Ticket");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketCode);

            entity.ToTable("Ticket");

            entity.Property(e => e.TicketCode).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("SYSTEM");
            entity.Property(e => e.EventDateEnd).HasDefaultValueSql("(sysdatetimeoffset())", "DF_Ticket_EventDateEnd");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TicketName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysdatetimeoffset())");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("SYSTEM");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
