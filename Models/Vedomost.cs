using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekanatUniversity.Models
{
    public class Vedomost
    {
        public int Id { get; set; }

        [Range(1, 12), Display(Name = "Семестр")]
        public int Semester { get; set; }

        [Display(Name = "Группа")]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public AcademicGroup? AcademicGroup { get; set; }

        [Display(Name = "Дисциплина")]
        public int DisciplineId { get; set; }

        [ForeignKey("DisciplineId")]
        public Discipline? Discipline { get; set; }

        [Display(Name = "Преподаватель")]
        public int TeacherId { get; set; } // Теперь int, а не string

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        [DataType(DataType.Date), Display(Name = "Дата создания")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Display(Name = "Статус")]
        public VedomostStatus Status { get; set; } = VedomostStatus.Created;

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }

    public enum VedomostStatus { Created, Filled, Approved, Canceled }
}