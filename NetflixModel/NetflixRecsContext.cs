using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NetflixModel;

public partial class NetflixRecsContext : IdentityDbContext<NetflixRecsUser> {
    public NetflixRecsContext(){
    }

    public NetflixRecsContext(DbContextOptions<NetflixRecsContext> options)
        : base(options){
    }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NetflixRecsFinal;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Show>(entity =>
        {
            entity.Property(e => e.GenreID).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
