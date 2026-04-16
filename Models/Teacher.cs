using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DekanatUniversity.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Навигационное свойство к IdentityUser
        public IdentityUser? User { get; set; }

        [Required, Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required, Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Display(Name = "Ученая степень")]
        public string? Degree { get; set; }

        [Display(Name = "Должность")]
        public string? Position { get; set; }

        [Display(Name = "Кафедра")]
        public string? Department { get; set; }

        public ICollection<Vedomost> Vedomosti { get; set; } = new List<Vedomost>();
    }
}