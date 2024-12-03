using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models;
using Microsoft.AspNetCore.Authorization;

namespace Grosu_Olesea_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Grosu_Olesea_Lab2Context _context;

        public CreateModel(Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            // Dropdown pentru Publisher
            ViewData["PublisherID"] = new SelectList(
                _context.Publisher.Select(p => new
                {
                    p.ID,
                    p.PublisherName
                }),
                "ID", "PublisherName");

            // Dropdown pentru Author
            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.LastName + " " + x.FirstName
            }).ToList();

            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");

            // Pregătește datele pentru categoriile asociate
            var book = new Book();
            book.BookCategories = new List<BookCategory>();
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            if (!ModelState.IsValid)
            {
                // Reîncarcă dropdown-urile și datele pentru checkbox-uri în caz de eroare de validare
                OnGet();
                return Page();
            }

            var newBook = new Book();
            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    var catToAdd = new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    };
                    newBook.BookCategories.Add(catToAdd);
                }
            }

            Book.BookCategories = newBook.BookCategories;
            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
