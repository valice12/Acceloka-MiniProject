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
            entity.HasKey(e => e.BookedId).HasName("PK__BookedTi__FA2CBA5A6978F290");

            entity.ToTable("BookedTicket");

            entity.HasOne(d => d.TicketCodeNavigation).WithMany(p => p.BookedTickets)
                .HasForeignKey(d => d.TicketCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookedTic__Ticke__4D94879B");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketCode).HasName("PK__Ticket__598CF7A20D8CBC40");

            entity.ToTable("Ticket");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EventDateStart).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.TicketName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
