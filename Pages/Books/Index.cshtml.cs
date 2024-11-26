using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models;

namespace Grosu_Olesea_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2Context _context;

        public IndexModel(Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; }
        public BookData BookD { get; set; } = new BookData();
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id, int? categoryID)
        {
            BookD.Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            if (id != null)
            {
                BookID = id.Value;
                Book selectedBook = BookD.Books
                    .Where(b => b.ID == id.Value)
                    .SingleOrDefault();

                if (selectedBook != null)
                {
                    BookD.Categories = selectedBook.BookCategories.Select(bc => bc.Category);
                }
            }
        }
    }
}
