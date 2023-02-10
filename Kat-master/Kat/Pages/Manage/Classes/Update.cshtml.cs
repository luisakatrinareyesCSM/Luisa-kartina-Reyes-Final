using Kat.Infrastructure.Domain;
using Kat.Infrastructure.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kat.Pages.Manage.Classes
{
    public class Update : PageModel
    {
        private ILogger<Index> _logger;
        private DefaultDbContext _context;

        [BindProperty]
        public ViewModel View { get; set; }

        public Update(DefaultDbContext context, ILogger<Index> logger)
        {
            _logger = logger;
            _context = context;
            View = View ?? new ViewModel();
        }

        public IActionResult OnGet(Guid? id = null)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clas = _context?.Classes?.Where(a => a.ClassId == id)
                                   .Select(a => new ViewModel()
                                   {
                                       ClassId = a.ClassId,
                                       Code = a.Code,
                                       YearLevel = a.YearLevel,
                                       StartDate = a.StartDate,
                                       Meeting = a.Meeting

                                   }).FirstOrDefault();

            if (clas == null)
            {
                return NotFound();
            }

            View = clas;
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

            var existingClass = _context?.Classes?.FirstOrDefault(a =>
                    a.ClassId != View.ClassId &&
                    a.Code.ToLower() == View.Code.ToLower()
            );

            if (existingClass != null)
            {
                ModelState.AddModelError("", "Class is already existing.");
                return Page();
            }

            var clas = _context?.Classes?.FirstOrDefault(a => a.ClassId == View.ClassId);

            if (clas != null)
            {
                clas.Code = View.Code;
                clas.YearLevel = View.YearLevel;
                clas.StartDate = View.StartDate;
                clas.Meeting = View.Meeting;




                _context?.Classes?.Update(clas);
                _context?.SaveChanges();

                return RedirectPermanent("~/manage/Patient");
            }

            return Page();

        }

        public class ViewModel : Class
        {

        }
    }
}
