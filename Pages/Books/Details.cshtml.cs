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
    public class DetailsModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context _context;

        public DetailsModel(Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public Models.Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include relațiile cu Author, Publisher și Categories
            Book = await _context.Book
                .Include(b => b.Author)    // Încarcă relația cu Author
                .Include(b => b.Publisher) // Încarcă relația cu Publisher
                .Include(b => b.BookCategories) // Încarcă relația BookCategories
                    .ThenInclude(bc => bc.Category) // Încarcă relația Category din BookCategories
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
