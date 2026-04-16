using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models
{
    public class Grade
    {
        public int Id { get; set; }

        public int VedomostId { get; set; }
        public Vedomost? Vedomost { get; set; }

        public int StudentId { get; set; }
        public Student? Student { get; set; }

        [Display(Name = "Оценка")]
        public string? Mark { get; set; }
    }
}