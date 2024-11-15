using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models;

namespace Grosu_Olesea_Lab2.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context _context;

        public CreateModel(Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Populează dropdown pentru Publisher cu o opțiune implicită
            ViewData["PublisherID"] = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "-- Select a Publisher --", Value = "" }
                }
                .Concat(_context.Set<Publisher>()
                    .Select(p => new SelectListItem
                    {
                        Text = p.PublisherName,
                        Value = p.ID.ToString()
                    })
                ).ToList(), "Value", "Text");

            // Populează dropdown pentru Author cu o opțiune implicită
            ViewData["AuthorID"] = new SelectList(
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "-- Select an Author --", Value = "" }
                }
                .Concat(_context.Set<Author>()
                    .Select(a => new SelectListItem
                    {
                        Text = $"{a.FirstName} {a.LastName}",
                        Value = a.ID.ToString()
                    })
                ).ToList(), "Value", "Text");

            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reîncarcă dropdown-urile în caz de eroare de validare
                OnGet();
                return Page();
            }

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
