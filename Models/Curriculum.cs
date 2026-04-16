using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models
{
    public class Curriculum
    {
        public int Id { get; set; }

        [Required]
        public string Specialty { get; set; } = string.Empty;

        public int Semester { get; set; }

        public int DisciplineId { get; set; }
        public Discipline? Discipline { get; set; }
    }
}