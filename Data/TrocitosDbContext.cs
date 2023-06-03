using Microsoft.EntityFrameworkCore;
using trocitos.Models;

public class TrocitosDbContext : DbContext

{

    public TrocitosDbContext(DbContextOptions<TrocitosDbContext> options)
        : base(options)
    {


    }


    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<Reserver> Reservers { get; set; } = null!;


}
