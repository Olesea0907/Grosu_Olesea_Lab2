using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Grosu_Olesea_Lab2.Models;

namespace Grosu_Olesea_Lab2.Data
{

    public class Grosu_Olesea_Lab2Context : DbContext
    {
        public Grosu_Olesea_Lab2Context(DbContextOptions<Grosu_Olesea_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Publisher> Publisher { get; set; } = default!;
        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<BookCategory> BookCategory { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookID, bc.CategoryID }); 

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCategories)
                .HasForeignKey(bc => bc.BookID);

            modelBuilder.Entity<BookCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookCategories)
                .HasForeignKey(bc => bc.CategoryID);
        }
    }

}
