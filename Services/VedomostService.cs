using DekanatUniversity.Data;
using DekanatUniversity.Models;
using DekanatUniversity.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DekanatUniversity.Services
{
    public class VedomostService
    {
        private readonly ApplicationDbContext _context;

        public VedomostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VedomostEditViewModel?> GetVedomostEditModelAsync(int id)
        {
            var vedomost = await _context.Vedomosti
                .Include(v => v.AcademicGroup)
                    .ThenInclude(g => g.Students)
                .Include(v => v.Discipline)
                .Include(v => v.Grades)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vedomost == null) return null;

            var model = new VedomostEditViewModel
            {
                VedomostId = vedomost.Id,
                Semester = vedomost.Semester,
                GroupName = vedomost.AcademicGroup?.Name ?? string.Empty,
                DisciplineName = vedomost.Discipline?.Name ?? string.Empty,
                Grades = new List<GradeEntry>()
            };

            if (vedomost.AcademicGroup != null)
            {
                foreach (var student in vedomost.AcademicGroup.Students.OrderBy(s => s.LastName))
                {
                    var existingGrade = vedomost.Grades.FirstOrDefault(g => g.StudentId == student.Id);
                    model.Grades.Add(new GradeEntry
                    {
                        StudentId = student.Id,
                        StudentFullName = $"{student.LastName} {student.FirstName} {student.MiddleName}",
                        RecordBookNumber = student.RecordBookNumber,
                        Mark = existingGrade?.Mark
                    });
                }
            }

            return model;
        }

        public async Task<bool> SaveGradesAsync(int vedomostId, List<GradeEntry> grades)
        {
            var vedomost = await _context.Vedomosti
                .Include(v => v.Grades)
                .FirstOrDefaultAsync(v => v.Id == vedomostId);

            if (vedomost == null || vedomost.Status == VedomostStatus.Approved)
                return false;

            foreach (var entry in grades)
            {
                if (string.IsNullOrEmpty(entry.Mark)) continue;
                var grade = vedomost.Grades.FirstOrDefault(g => g.StudentId == entry.StudentId);
                if (grade == null)
                {
                    grade = new Grade { StudentId = entry.StudentId, VedomostId = vedomostId };
                    _context.Grades.Add(grade);
                }
                grade.Mark = entry.Mark;
            }

            vedomost.Status = VedomostStatus.Filled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Vedomost>> GetAvailableVedomostiForTeacherAsync(string userId)
        {
            // Находим преподавателя по UserId
            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
                return new List<Vedomost>();

            return await _context.Vedomosti
                .Include(v => v.AcademicGroup)
                .Include(v => v.Discipline)
                .Where(v => v.TeacherId == teacher.Id && v.Status != VedomostStatus.Approved)
                .ToListAsync();
        }
    }
}