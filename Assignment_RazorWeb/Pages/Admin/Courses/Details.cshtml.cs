using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Assignment_RazorWeb.Pages.Courses
{
    public class DetailModel : PageModel
    {
        private readonly ICourseService _courseService;

        public DetailModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Course = await _courseService.GetCourseByIdAsync(id);

            if (Course == null || !Course.IsActive) 
            {
                return NotFound();
            }

            return Page();
        }
    }
}
