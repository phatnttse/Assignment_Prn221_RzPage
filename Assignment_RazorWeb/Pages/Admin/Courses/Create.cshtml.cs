using Assignment_BusinessObjects;
using Assignment_Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Assignment_RazorWeb.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly ICourseService _courseService;

        public CreateModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Fields is required")]
        public Course Course { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImageFile { get; set; }

        public string ErrorMessage { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var folderPath = Path.Combine("wwwroot", "images");

                    // Kiểm tra và tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    var domain = $"{Request.Scheme}://{Request.Host}";
                    Course.ImageUrl = $"{domain}/images/{fileName}";
                }

                await _courseService.AddCourseAsync(Course);
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while creating the course: " + ex.Message;
                return Page();
            }

            return RedirectToPage("Index");
        }

    }
}
