using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Grosu_Olesea_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context _context;

        public DeleteModel(Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Book
                .Include(b => b.Author)    // Include relația cu Author
                .Include(b => b.Publisher) // Include relația cu Publisher
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category) // Include categoriile
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book = await _context.Book
                .Include(b => b.BookCategories) 
                .FirstOrDefaultAsync(b => b.ID == id);

            if (Book != null)
            {
                _context.RemoveRange(Book.BookCategories);

                _context.Book.Remove(Book);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
