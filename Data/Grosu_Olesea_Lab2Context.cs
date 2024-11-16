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
        public Grosu_Olesea_Lab2Context (DbContextOptions<Grosu_Olesea_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Grosu_Olesea_Lab2.Models.Book> Book { get; set; } = default!;
        public DbSet<Grosu_Olesea_Lab2.Models.Publisher> Publisher { get; set; } = default!;
        public DbSet<Grosu_Olesea_Lab2.Models.Author> Author { get; set; } = default!;
        public DbSet<Grosu_Olesea_Lab2.Models.Category> Category { get; set; } = default!;
    }
}
