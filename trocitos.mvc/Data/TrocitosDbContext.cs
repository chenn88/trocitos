using Microsoft.EntityFrameworkCore;
using trocitos.mvc.Models;

public class TrocitosDbContext : DbContext

{

    public TrocitosDbContext() { }
    public TrocitosDbContext(DbContextOptions<TrocitosDbContext> options)
        : base(options)
    {


    }


    public virtual DbSet<Reservation> Reservations { get; set; } = null!;
    public virtual DbSet<Table> TableCatalogue { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(r => r.TableNo);
    }
}
