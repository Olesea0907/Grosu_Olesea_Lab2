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
        private readonly Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context _context;

        public IndexModel(Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public IList<Models.Book> Book { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Book = await _context.Book
                .Include(b=>b.Author)
                .Include(b=>b.Publisher).ToListAsync();

        }
    }
}
