using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment_RazorWeb.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IList<Course> Courses { get; private set; }

        public async Task OnGetAsync()
        {
            Courses = (await _courseService.GetActiveCoursesAsync()).ToList();
        }
    }
}
