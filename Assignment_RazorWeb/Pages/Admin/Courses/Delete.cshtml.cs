using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Assignment_RazorWeb.Pages.Courses
{
    public class DeleteModel : PageModel
    {
        private readonly ICourseService _courseService;

        public DeleteModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Course = await _courseService.GetCourseByIdAsync(id);

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Course != null)
            {
                var result = await _courseService.DeleteCourse(Course.Id);
                if (!result)
                {
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}
