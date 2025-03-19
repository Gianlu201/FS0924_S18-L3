using FS0924_S18_L3.Services;
using FS0924_S18_L3.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FS0924_S18_L3.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllStudents()
        {
            var studentsList = await _studentService.GetAllStudentsAsync();

            return PartialView("_StudentsList", studentsList);
        }

        [HttpGet("student/add")]
        public IActionResult Add()
        {
            return PartialView(
                "_AddStudent",
                new AddStudentViewModel()
                {
                    Name = "",
                    Surname = "",
                    Email = "",
                }
            );
        }

        [HttpPost("student/add/save")]
        public async Task<IActionResult> AddSave(AddStudentViewModel addStudentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(
                    new { success = false, message = "Error while adding entity on database" }
                );
            }

            var result = await _studentService.AddStudentAsync(addStudentViewModel);

            var resultMessage = "Student added successfully";

            if (!result)
            {
                resultMessage = "Error while adding entity on database";
            }

            return Json(new { success = result, message = resultMessage });
        }

        [HttpGet("student/edit/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
            {
                return RedirectToAction("Index");
            }

            var editStudent = new EditStudentViewModel()
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                BirthdayDate = student.BirthdayDate,
                Email = student.Email,
            };

            return PartialView("_EditStudent", editStudent);
        }

        [HttpPost("student/edit/save/")]
        public async Task<IActionResult> EditSave(EditStudentViewModel editStudentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(
                    new { success = false, message = "Error while updating entity on database" }
                );
            }

            var result = await _studentService.UpdateStudentAsync(editStudentViewModel);

            var ResultMessage = "Entity successfully updated";

            if (!result)
            {
                ResultMessage = "Error while updating entity on database";
            }

            return Json(new { success = result, message = ResultMessage });
        }

        [HttpPost("student/delete")]
        public IActionResult ShowDeleteModal()
        {
            return PartialView("_DeleteStudentModal");
        }

        [HttpPost("student/delete/save/{id:guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var result = await _studentService.DeleteStudentByIdAsync(id);

            var resultMessage = "Entity deleted successfully";

            if (!result)
            {
                resultMessage = "Error while deleting entity from database";
            }

            return Json(new { success = result, message = resultMessage });
        }
    }
}
