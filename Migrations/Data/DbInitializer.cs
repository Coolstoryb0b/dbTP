using DekanatUniversity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DekanatUniversity.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Применяем все миграции (создаёт базу, если её нет)
            await context.Database.MigrateAsync();

            // 1. Создаём роли, если их нет
            string[] roles = { "Admin", "Deanery", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Создаём администратора (если нет)
            var adminEmail = "admin@dekanat.ru";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 3. Создаём тестовых пользователей: преподаватель, студент, сотрудник деканата
            var teacherEmail = "teacher@dekanat.ru";
            IdentityUser? teacherUser = await userManager.FindByEmailAsync(teacherEmail);
            if (teacherUser == null)
            {
                teacherUser = new IdentityUser
                {
                    UserName = teacherEmail,
                    Email = teacherEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(teacherUser, "Teacher123!");
                await userManager.AddToRoleAsync(teacherUser, "Teacher");
            }

            var studentEmail = "student@dekanat.ru";
            IdentityUser? studentUser = await userManager.FindByEmailAsync(studentEmail);
            if (studentUser == null)
            {
                studentUser = new IdentityUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(studentUser, "Student123!");
                await userManager.AddToRoleAsync(studentUser, "Student");
            }

            var deanEmail = "dean@dekanat.ru";
            IdentityUser? deanUser = await userManager.FindByEmailAsync(deanEmail);
            if (deanUser == null)
            {
                deanUser = new IdentityUser
                {
                    UserName = deanEmail,
                    Email = deanEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(deanUser, "Dean123!");
                await userManager.AddToRoleAsync(deanUser, "Deanery");
            }

            // 4. Создаём группы, если их нет
            if (!await context.AcademicGroups.AnyAsync())
            {
                var group1 = new AcademicGroup
                {
                    Name = "ИСТ-119",
                    Specialty = "Информационные системы и технологии",
                    Year = 2019
                };
                var group2 = new AcademicGroup
                {
                    Name = "ПМИ-120",
                    Specialty = "Прикладная математика и информатика",
                    Year = 2020
                };
                context.AcademicGroups.AddRange(group1, group2);
                await context.SaveChangesAsync();
            }

            // 5. Создаём дисциплины
            if (!await context.Disciplines.AnyAsync())
            {
                var disc1 = new Discipline
                {
                    Name = "Программирование",
                    LectureHours = 36,
                    PracticeHours = 36,
                    LabHours = 18,
                    ControlType = ControlType.Exam
                };
                var disc2 = new Discipline
                {
                    Name = "Базы данных",
                    LectureHours = 34,
                    PracticeHours = 34,
                    LabHours = 17,
                    ControlType = ControlType.Credit
                };
                context.Disciplines.AddRange(disc1, disc2);
                await context.SaveChangesAsync();
            }

            // 6. Создаём преподавателя (связываем с IdentityUser)
            Teacher? teacher = await context.Teachers
                .FirstOrDefaultAsync(t => t.UserId == teacherUser.Id);
            if (teacher == null)
            {
                teacher = new Teacher
                {
                    UserId = teacherUser.Id,
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    Degree = "канд. техн. наук",
                    Position = "доцент",
                    Department = "ИСПИ"
                };
                context.Teachers.Add(teacher);
                await context.SaveChangesAsync();
            }

            // 7. Создаём студента (связываем с IdentityUser и группой)
            var groupIst = await context.AcademicGroups.FirstOrDefaultAsync(g => g.Name == "ИСТ-119");
            Student? student = await context.Students
                .FirstOrDefaultAsync(s => s.UserId == studentUser.Id);
            if (student == null && groupIst != null)
            {
                student = new Student
                {
                    UserId = studentUser.Id,
                    LastName = "Петров",
                    FirstName = "Пётр",
                    MiddleName = "Петрович",
                    BirthDate = new DateTime(2001, 5, 15),
                    RecordBookNumber = "123456",
                    GroupId = groupIst.Id
                };
                context.Students.Add(student);
                await context.SaveChangesAsync();
            }

            // 8. Создаём ведомость (если ещё нет)
            var disciplineProg = await context.Disciplines
                .FirstOrDefaultAsync(d => d.Name == "Программирование");
            if (!await context.Vedomosti.AnyAsync() && groupIst != null && teacher != null && disciplineProg != null)
            {
                var vedomost = new Vedomost
                {
                    Semester = 1,
                    GroupId = groupIst.Id,
                    DisciplineId = disciplineProg.Id,
                    TeacherId = teacher.Id,
                    DateCreated = DateTime.Now,
                    Status = VedomostStatus.Created
                };
                context.Vedomosti.Add(vedomost);
                await context.SaveChangesAsync();

                // Добавляем оценку для нашего студента
                var grade = new Grade
                {
                    VedomostId = vedomost.Id,
                    StudentId = student.Id,
                    Mark = "отлично"
                };
                context.Grades.Add(grade);
                await context.SaveChangesAsync();

                // Меняем статус ведомости на Filled
                vedomost.Status = VedomostStatus.Filled;
                context.Update(vedomost);
                await context.SaveChangesAsync();
            }
        }
    }
}