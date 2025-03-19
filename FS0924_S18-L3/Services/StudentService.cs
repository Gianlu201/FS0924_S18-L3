using FS0924_S18_L3.Data;
using FS0924_S18_L3.Models;
using FS0924_S18_L3.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FS0924_S18_L3.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly LoggerService _loggerService;

        public StudentService(ApplicationDbContext context, LoggerService loggerService)
        {
            _context = context;
            _loggerService = loggerService;
        }

        private async Task<bool> TrySaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<StudentsListViewModel> GetAllStudentsAsync()
        {
            try
            {
                var studentsList = new StudentsListViewModel();

                studentsList.Students = await _context.Students.ToListAsync();

                _loggerService.LogInformation("Students list requested by admin");
                return studentsList;
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return new StudentsListViewModel() { Students = new List<Student>() };
            }
        }

        public async Task<Student> GetStudentByIdAsync(Guid id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);

                if (student == null)
                {
                    return new Student();
                }

                var foundStudent = new Student()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Surname = student.Surname,
                    BirthdayDate = student.BirthdayDate,
                    Email = student.Email,
                };

                _loggerService.LogInformation("Single student requested by admin");
                return foundStudent;
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return new Student();
            }
        }

        public async Task<bool> AddStudentAsync(AddStudentViewModel addStudentViewModel)
        {
            try
            {
                var student = new Student()
                {
                    Id = Guid.NewGuid(),
                    Name = addStudentViewModel.Name,
                    Surname = addStudentViewModel.Surname,
                    BirthdayDate = addStudentViewModel.BirthdayDate,
                    Email = addStudentViewModel.Email,
                };

                var result = _context.Add(student);

                _loggerService.LogInformation("Student added in database by admin");
                return await TrySaveAsync();
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateStudentAsync(EditStudentViewModel editStudentViewModel)
        {
            try
            {
                var result = await _context.Students.FindAsync(editStudentViewModel.Id);

                if (result == null)
                {
                    return false;
                }

                result.Name = editStudentViewModel.Name;
                result.Surname = editStudentViewModel.Surname;
                result.BirthdayDate = editStudentViewModel.BirthdayDate;
                result.Email = editStudentViewModel.Email;

                _loggerService.LogInformation("Student updated by admin");
                return await TrySaveAsync();
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteStudentByIdAsync(Guid id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);

                if (student == null)
                {
                    return false;
                }

                _context.Students.Remove(student);

                _loggerService.LogWarning("Student deleted by admin");
                return await TrySaveAsync();
            }
            catch (Exception ex)
            {
                _loggerService.LogError(ex.Message);
                return false;
            }
        }
    }
}
