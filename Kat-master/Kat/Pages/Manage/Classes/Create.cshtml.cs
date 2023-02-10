using Kat.Infrastructure.Domain;
using Kat.Infrastructure.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kat.Pages.Manage.Classes
{
    public class Create : PageModel
    {
        private ILogger<Index> _logger;
        private DefaultDbContext _context;

        [BindProperty]
        public ViewModel View { get; set; }

        public Create(DefaultDbContext context, ILogger<Index> logger)
        {
            _logger = logger;
            _context = context;
            View = View ?? new ViewModel();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(View.Code))
            {
                ModelState.AddModelError("", "code cannot be blank.");
                return Page();
            }
            if (string.IsNullOrEmpty(View.YearLevel))
            {
                ModelState.AddModelError("", "Yaer Level cannot be blank.");
                return Page();
            }

            if (string.IsNullOrEmpty(View.Code))
            {
                ModelState.AddModelError("", " be blank.");
                return Page();
            }
            if (!Enum.IsDefined(typeof(Enum), View.Meeting))
            {
                ModelState.AddModelError("", "Meeting cannot be blank.");
                return Page();
            }
            if (DateTime.MinValue >= View.StartDate)
            {
                ModelState.AddModelError("", "StartDate cannot be blank.");
                return Page();
            }

            var existingClass = _context?.Classes?.FirstOrDefault(a => a.Code.ToLower() == View.Code.ToLower());
            if (existingClass != null)
            {
                ModelState.AddModelError("", "is already existing.");
                return Page();
            }
            

            Class classes = new Class()
            {
                ClassId = Guid.NewGuid(),
                Code = View.Code,
                YearLevel = View.YearLevel,
                Meeting = View.Meeting,
                StartDate = View.StartDate







            };


            _context?.Classes?.Add(classes);

            _context?.SaveChanges();

            return RedirectPermanent("~/manage/classes");
        }

        public class ViewModel : Class
        {

        }
    }
}