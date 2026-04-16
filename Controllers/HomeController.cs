using System.Diagnostics;
using System.Security.Claims;
using DekanatUniversity.Models;
using DekanatUniversity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DekanatUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyGrades()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = await _context.Students
                .Include(s => s.Grades)
                    .ThenInclude(g => g.Vedomost)
                        .ThenInclude(v => v.Discipline)
                .Include(s => s.Grades)
                    .ThenInclude(g => g.Vedomost)
                        .ThenInclude(v => v.Teacher)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
                return NotFound("Студент не найден.");

            return View(student.Grades.OrderByDescending(g => g.Vedomost?.Semester).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}