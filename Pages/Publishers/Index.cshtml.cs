using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Grosu_Olesea_Lab2.Data;
using Grosu_Olesea_Lab2.Models;
using Grosu_Olesea_Lab2.Models.ViewModels;

namespace Grosu_Olesea_Lab2.Pages.Publishers
{
    public class IndexModel : PageModel
    {
        private readonly Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context _context;

        public IndexModel(Grosu_Olesea_Lab2.Data.Grosu_Olesea_Lab2Context context)
        {
            _context = context;
        }

        public PublisherIndexData PublisherData { get; set; } = new PublisherIndexData();
        public int PublisherID { get; set; }
        public int BookID { get; set; }

        public async Task OnGetAsync(int? id, int? bookID)
        {
            PublisherData.Publishers = await _context.Publisher
                .Include(i => i.Books)
                .ThenInclude(b => b.Author)
                .OrderBy(i => i.PublisherName)
                .ToListAsync();

            if (id != null)
            {
                PublisherID = id.Value;

                var selectedPublisher = PublisherData.Publishers
                    .SingleOrDefault(i => i.ID == id.Value);

                if (selectedPublisher != null)
                {
                    PublisherData.Books = selectedPublisher.Books;
                }
            }
        }
    }
}
