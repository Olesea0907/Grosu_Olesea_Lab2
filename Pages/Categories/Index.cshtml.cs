using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Namespace-ul adăugat

namespace Grosu_Olesea_Lab2.Pages.Categories
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2Context _context;

        public IndexModel(Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public CategoryIndexData CategoryData { get; set; } = new CategoryIndexData();
        public int CategoryID { get; set; }

        public async Task OnGetAsync(int? id)
        {
            CategoryData.Categories = await _context.Category
                .Include(c => c.BookCategories)
                .ThenInclude(bc => bc.Book)
                .ThenInclude(b => b.Author)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            if (id != null)
            {
                CategoryID = id.Value;
                var selectedCategory = CategoryData.Categories
                    .Where(c => c.ID == id.Value)
                    .SingleOrDefault();

                if (selectedCategory != null)
                {
                    CategoryData.Books = selectedCategory.BookCategories
                        .Select(bc => bc.Book);
                }
            }
        }
    }
}
