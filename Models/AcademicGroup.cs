using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models
{
    public class AcademicGroup
    {
        public int Id { get; set; }

        [Required, Display(Name = "Название группы")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Специальность")]
        public string? Specialty { get; set; }

        [Display(Name = "Год поступления")]
        public int Year { get; set; }

        [Display(Name = "Куратор")]
        public int? CuratorId { get; set; }
        public Teacher? Curator { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Vedomost> Vedomosti { get; set; } = new List<Vedomost>();
    }
}