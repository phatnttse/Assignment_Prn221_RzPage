using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;

namespace Assignment_RazorWeb.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly ICourseService _courseService;

        public EditModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; } // Thêm trường cho hình ảnh mới

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Xử lý tải ảnh nếu người dùng chọn hình ảnh mới
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                var domain = $"{Request.Scheme}://{Request.Host}";
                Course.ImageUrl = $"{domain}/images/{fileName}"; 
            }

            // Cập nhật thông tin khóa học
            await _courseService.UpdateCourseAsync(Course);

            return RedirectToPage("Index");
        }
    }
}
