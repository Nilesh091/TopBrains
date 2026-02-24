using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Course_Enrollment_System.Model;
using Online_Course_Enrollment_System.Services;

namespace Online_Course_Enrollment_System.Controllers
{
    [Route("api/enrollment")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentManagementService enrollmentManagementService;

        public EnrollmentController(IEnrollmentManagementService enrollmentManagementService)
        {
            this.enrollmentManagementService = enrollmentManagementService;
        }
        [HttpPost]
        public IActionResult EnrollStudentInCourse(Enrollment em)
        {
            var enr = enrollmentManagementService.EnrollStudentInCourse(em);
            return Ok(enr);
        }
        [HttpGet]
        public IActionResult GetAllEnrollments()
        {
            return Ok(enrollmentManagementService.GetAllEnrollments());
        }
        [HttpGet("{studentId}")]
        public IActionResult GetAllEnroledCourse(int studentId)
        {
            return Ok(enrollmentManagementService.GetAllEnroledCourse(studentId));
        }
        [HttpGet("{courseId}")]
        public IActionResult GetAllStudentsEnrolled(int courseId)
        {
            return Ok(enrollmentManagementService.GetAllStudentsEnrolled(courseId));
        }
    }
}
