using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment_RazorWeb.Pages
{
    public class MyEnrollmentModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICourseService _courseService;
        private readonly IUserIdAccessor _userIdAccessor;
        private readonly IEnrollmentService _enrollmentService;

        [BindProperty]
        public string CourseId { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; } = Enumerable.Empty<Enrollment>();

        public MyEnrollmentModel(ILogger<IndexModel> logger, ICourseService courseService, IUserIdAccessor userIdAccessor, IEnrollmentService enrollmentService)
        {
            _logger = logger;
            _courseService = courseService;
            _userIdAccessor = userIdAccessor;
            _enrollmentService = enrollmentService;
        }

        public async Task OnGetAsync()
        {
            string userId = _userIdAccessor.GetCurrentUserId();
            Enrollments = await _enrollmentService.GetEnrollmentsByUserAsync(userId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userIdAccessor.GetCurrentUserId();

            if (string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "User ID not found.";
                return Page();
            }

            if (string.IsNullOrEmpty(CourseId))
            {
                ErrorMessage = "Course ID is required.";
                return Page();
            }

            var response = await _enrollmentService.UserUnenrollFromCourse(userId, CourseId);

            if (response.Succeeded)
            {
                SuccessMessage = response.Message;
            }
            else
            {
                ErrorMessage = response.Message;
            }

            return Page();
        }
    }
    }
