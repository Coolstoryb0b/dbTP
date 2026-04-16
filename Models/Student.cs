using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public IdentityUser? User { get; set; }

        [Required, Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required, Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [DataType(DataType.Date), Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required, Display(Name = "№ зачетной книжки")]
        public string RecordBookNumber { get; set; } = string.Empty;

        [Display(Name = "Группа")]
        public int GroupId { get; set; }

        public AcademicGroup? AcademicGroup { get; set; }

        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}