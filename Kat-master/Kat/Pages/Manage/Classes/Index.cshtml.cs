using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Kat.Infrastructure.Domain;
using Kat.Infrastructure.Domain.Models;

namespace Kat.Pages.Manage.Classes
{
    [Authorize(Roles = "Admin")]
    public class Index : PageModel
    {
        private ILogger<Index> _logger;
        private DefaultDbContext _context;

        [BindProperty]
        public ViewModel View { get; set; }

        public Index(DefaultDbContext context, ILogger<Index> logger)
        {
            _logger = logger;
            _context = context;
            View = View ?? new ViewModel();
        }

        public IActionResult OnGet(int? pageIndex = 1, int? pageSize = 10, string? sortBy = "", SortOrder sortOrder = SortOrder.Ascending, string? keyword = "")
        {
            var skip = (int)((pageIndex - 1) * pageSize);

            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a =>
                            a.YearLevel != null && a.YearLevel.ToLower().Contains(keyword.ToLower())
                        || a.Code != null && a.Code.ToLower().Contains(keyword.ToLower())

                );
            }

            var totalRows = query.Count();

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "year" && sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(a => a.YearLevel);
                }
                else if (sortBy.ToLower() == "year" && sortOrder == SortOrder.Descending)
                {
                    query = query.OrderByDescending(a => a.YearLevel);
                }
                else if (sortBy.ToLower() == "code" && sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(a => a.Code);
                }
                else if (sortBy.ToLower() == "code" && sortOrder == SortOrder.Descending)
                {
                    query = query.OrderByDescending(a => a.Code);
                }

            }

            var classes = query
                            .Skip(skip)
                            .Take((int)pageSize)
                            .ToList();

            View.Classes = new Paged<Class>()
            {
                Items = classes,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRows = totalRows,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Keyword = keyword
            };

            return Page();
        }

        public class ViewModel
        {
            public Paged<Class>? Classes { get; set; }
        }
    }
}
