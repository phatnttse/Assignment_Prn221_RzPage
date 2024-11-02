using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Assignment_RazorWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICourseService _courseService;
        private readonly IUserIdAccessor _userIdAccessor;
        private readonly IEnrollmentService _enrollmentService;

        [BindProperty]
        public string CourseId { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<Course> Courses { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ICourseService courseService, IUserIdAccessor userIdAccessor, IEnrollmentService enrollmentService)
        {
            _logger = logger;
            _courseService = courseService;
            _userIdAccessor = userIdAccessor;
            _enrollmentService = enrollmentService;
        }

        public async Task OnGetAsync()
        {
            Courses = await _courseService.GetAllCoursesAsync();
        }

        public async Task OnPostAsync()
        {
            Courses = await _courseService.GetAllCoursesAsync();

            if (!ModelState.IsValid)
            {
                return; 
            }

            string userId = _userIdAccessor.GetCurrentUserId();


            if (string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Bạn cần đăng nhập để thực hiện việc này.";
                return; 
            }

            var check = await _enrollmentService.IsUserEnrollmented(userId, CourseId);

            if (check)
            {
                ErrorMessage = "Bạn đã đăng ký khóa học này rồi.";
                return;
            }

            var result = await _enrollmentService.UserEnrollIntoCourse(userId, CourseId);

            if (result.Succeeded)
            {
                SuccessMessage = result.Message;
            }
            else
            {
                ErrorMessage = result.Message; 
            }
        }
    }
}
