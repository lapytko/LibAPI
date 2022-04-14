using System;
using System.IO;
using LibAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibAPI.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {

        }

        // public DbSet<ApplicationFunction> ApplicationFunctions { get; set; }
        // public DbSet<ApplicationMenu> ApplicationMenus { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Style> Styles { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Style> Counties { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookScore> BookScores { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Lending> Lendings { get; set; }
        
        public DbSet<LendingItem> LendingItems { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
#if DEBUG
            var connectionString = config.GetConnectionString("TestConnection");
#else
            var connectionString = config.GetConnectionString("ProdConnection");
#endif
            optionsBuilder.UseSqlServer(connectionString,
                contextOptionsBuilder =>
                    contextOptionsBuilder.CommandTimeout((int)TimeSpan.FromHours(24).TotalSeconds));
            //base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Book>()
                .HasMany(c => c.Author)
                .WithMany(s => s.Books);

            modelBuilder
                .Entity<Book>()
                .HasMany(c => c.Style)
                .WithMany(s => s.Books);
            
            modelBuilder
                .Entity<Book>()
                .HasMany(c => c.LendingItem)
                .WithOne(s => s.Book)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Lending>()
                .HasMany(c => c.Items)
                .WithOne(s => s.Lending)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder
                .Entity<Lending>()
                .HasOne(c => c.Client)
                .WithMany(s => s.Lendings)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder
                .Entity<Lending>()
                .HasOne(c => c.Creator)
                .WithMany(s => s.Lendings)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            
            modelBuilder
                .Entity<Client>()
                .HasOne(c => c.Passport)
                .WithOne(s => s.Client)
                .HasForeignKey<Passport>(pt => pt.Id)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
