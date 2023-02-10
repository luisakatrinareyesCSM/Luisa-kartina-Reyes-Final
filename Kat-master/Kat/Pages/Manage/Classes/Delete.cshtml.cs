using Kat.Infrastructure.Domain;
using Kat.Infrastructure.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kat.Pages.Manage.Classes
{
    public class Delete : PageModel
    {
        private ILogger<Index> _logger;
        private DefaultDbContext _context;

        [BindProperty]
        public ViewModel View { get; set; }

        public Delete(DefaultDbContext context, ILogger<Index> logger)
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
            if (View.ClassId == null)
            {
                return NotFound();
            }

            var clas = _context?.Classes?.FirstOrDefault(a => a.ClassId == View.ClassId);

            if (clas != null)
            {
                _context?.Classes?.Remove(clas);
                _context?.SaveChanges();

                return RedirectPermanent("~/manage/Classe");
            }

            return NotFound();

        }

        public class ViewModel : Class
        {

        }
    }
}
