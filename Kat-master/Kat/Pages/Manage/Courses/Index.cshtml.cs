using Kat.Infrastructure.Domain;
using Kat.Infrastructure.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Kat.Pages.Manage.Courses
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

            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(a =>
                            a.Tittle != null && a.Tittle.ToLower().Contains(keyword.ToLower())
                        || a.Description != null && a.Description.ToLower().Contains(keyword.ToLower())
                        || a.Abbrevitation != null && a.Abbrevitation.ToLower().Contains(keyword.ToLower())
                );
            }

            var totalRows = query.Count();

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "tittle" && sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(a => a.Tittle);
                }
                else if (sortBy.ToLower() == "tittle" && sortOrder == SortOrder.Descending)
                {
                    query = query.OrderByDescending(a => a.Tittle);
                }
                else if (sortBy.ToLower() == "description" && sortOrder == SortOrder.Ascending)
                {
                    query = query.OrderBy(a => a.Description);
                }
                else if (sortBy.ToLower() == "description" && sortOrder == SortOrder.Descending)
                {
                    query = query.OrderByDescending(a => a.Description);
                }
                else if (sortBy.ToLower() == "abbreviation" && sortOrder == SortOrder.Ascending)
                {
                    query = _context.Courses.OrderBy(a => a.Abbrevitation);
                }
                else if (sortBy.ToLower() == "abbreviation" && sortOrder == SortOrder.Descending)
                {
                    query = query.OrderByDescending(a => a.Abbrevitation);
                }
            }

            var courses = query
                            .Skip(skip)
                            .Take((int)pageSize)
                            .ToList();

            View.Courses = new Paged<Course>()
            {
                Items = courses,
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
            public Paged<Course>? Courses { get; set; }
        }
    }
}
