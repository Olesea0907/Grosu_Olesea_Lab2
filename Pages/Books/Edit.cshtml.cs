using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models;

namespace Grosu_Olesea_Lab2.Pages.Books
{
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Grosu_Olesea_Lab2Context _context;

        public EditModel(Grosu_Olesea_Lab2Context context)
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
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null)
            {
                return NotFound();
            }

            PopulateAssignedCategoryData(_context, Book);

            var authorList = _context.Author.Select(a => new
            {
                a.ID,
                FullName = a.LastName + " " + a.FirstName
            }).ToList();
            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");

            // Creează lista pentru dropdown-ul Publisher
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include relațiile necesare
            var bookToUpdate = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            selectedCategories ??= Array.Empty<string>();

            // Actualizează datele din formular
            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                b => b.Title, b => b.AuthorID,
                b => b.Price, b => b.PublishingDate, b => b.PublisherID))
            {
                // Actualizează categoriile asociate
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // În caz de eroare, reîncarcă datele
            PopulateAssignedCategoryData(_context, bookToUpdate);
            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName", bookToUpdate.PublisherID);
            ViewData["AuthorID"] = new SelectList(
                _context.Author.Select(a => new
                {
                    a.ID,
                    FullName = a.LastName + " " + a.FirstName
                }),
                "ID", "FullName", bookToUpdate.AuthorID);

            return Page();
        }
    }
}
