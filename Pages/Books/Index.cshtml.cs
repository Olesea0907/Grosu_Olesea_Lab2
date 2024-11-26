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

        // Properties
        public IList<Book> Book { get; set; }
        public BookData BookD { get; set; } = new BookData();
        public int BookID { get; set; }
        public int CategoryID { get; set; }
        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }
        public string CurrentSort { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }

        // Method: OnGetAsync
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            // Sorting options
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";
            CurrentSort = sortOrder;
            CurrentFilter = searchString;

            // Query for books with relationships
            var booksQuery = _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b =>
                    b.Title.Contains(searchString) ||
                    b.Author.FirstName.Contains(searchString) ||
                    b.Author.LastName.Contains(searchString));
            }

            // Load books into memory for sorting by FullName
            var books = await booksQuery.AsNoTracking().ToListAsync();

            // Apply sorting
            books = sortOrder switch
            {
                "title_desc" => books.OrderByDescending(b => b.Title).ToList(),
                "author_desc" => books.OrderByDescending(b => b.Author.FullName).ToList(),
                "author" => books.OrderBy(b => b.Author.FullName).ToList(),
                _ => books.OrderBy(b => b.Title).ToList(),
            };

            // Assign sorted and filtered books
            BookD.Books = books;

            // Handle selected book and associated categories
            if (id != null)
            {
                BookID = id.Value;
                var selectedBook = BookD.Books.FirstOrDefault(b => b.ID == id.Value);
                if (selectedBook != null)
                {
                    BookD.Categories = selectedBook.BookCategories.Select(bc => bc.Category);
                }
            }
        }
    }
}
