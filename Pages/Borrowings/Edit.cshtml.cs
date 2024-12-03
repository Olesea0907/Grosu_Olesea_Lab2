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

namespace Grosu_Olesea_Lab2.Pages.Borrowings
{
    public class EditModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2Context _context;

        public EditModel(Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Borrowing Borrowing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obține împrumutul curent
            Borrowing = await _context.Borrowing
                .Include(b => b.Member)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Borrowing == null)
            {
                return NotFound();
            }

            // Pregătește lista de cărți cu detalii despre titlu și autor
            var bookList = _context.Book
                .Include(b => b.Author)
                .Select(x => new
                {
                    x.ID,
                    BookFullName = x.Title + " - " + x.Author.LastName + " " + x.Author.FirstName
                })
                .ToList();

            // Configurează ViewData pentru cărți și membri
            ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", Borrowing.BookID);
            ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName", Borrowing.MemberID);

            return Page();
        }

        // Protecție împotriva atacurilor de supra-postare
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reîncarcă listele derulante dacă modelul este invalid
                var bookList = _context.Book
                    .Include(b => b.Author)
                    .Select(x => new
                    {
                        x.ID,
                        BookFullName = x.Title + " - " + x.Author.LastName + " " + x.Author.FirstName
                    })
                    .ToList();

                ViewData["BookID"] = new SelectList(bookList, "ID", "BookFullName", Borrowing.BookID);
                ViewData["MemberID"] = new SelectList(_context.Member, "ID", "FullName", Borrowing.MemberID);

                return Page();
            }

            _context.Attach(Borrowing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowingExists(Borrowing.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowing.Any(e => e.ID == id);
        }
    }
}
