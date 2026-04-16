using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models
{
    public class Discipline
    {
        public int Id { get; set; }

        [Required, Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Лекции (часы)")]
        public int LectureHours { get; set; }

        [Display(Name = "Практики (часы)")]
        public int PracticeHours { get; set; }

        [Display(Name = "Лабораторные (часы)")]
        public int LabHours { get; set; }

        [Display(Name = "Форма контроля")]
        public ControlType ControlType { get; set; }

        public ICollection<Curriculum> Curriculums { get; set; } = new List<Curriculum>();
        public ICollection<Vedomost> Vedomosti { get; set; } = new List<Vedomost>();
    }

    public enum ControlType { Exam, Credit }
}