using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Online_Course_Enrollment_System.Model;
using Online_Course_Enrollment_System.Services;

namespace Online_Course_Enrollment_System.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentManagementService studentManagementService;

        public StudentController(IStudentManagementService sms)
        {
            this.studentManagementService = sms;
        }
        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            var returnedStudent = studentManagementService.AddStudent(student);
            return Ok(returnedStudent);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStudentDetails(int id, Student student)
        {
            if (student.Id != id)
            {
                return BadRequest();
            }
            var updatedStudent = studentManagementService.UpdateStudentDetails(id, student);
            return Ok(updatedStudent);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var deletedStudent = studentManagementService.DeleteStudent(id);
            if (deletedStudent != null)
            {
                return Ok(deletedStudent);
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(studentManagementService.GetAll());
        }
    }
}
