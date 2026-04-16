using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models.ViewModels
{
    public class VedomostEditViewModel
    {
        public int VedomostId { get; set; }

        [Display(Name = "Семестр")]
        public int Semester { get; set; }

        [Display(Name = "Группа")]
        public string GroupName { get; set; } = string.Empty;

        [Display(Name = "Дисциплина")]
        public string DisciplineName { get; set; } = string.Empty;

        public List<GradeEntry> Grades { get; set; } = new List<GradeEntry>();
    }

    public class GradeEntry
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; } = string.Empty;
        public string RecordBookNumber { get; set; } = string.Empty;
        public string? Mark { get; set; }
    }
}