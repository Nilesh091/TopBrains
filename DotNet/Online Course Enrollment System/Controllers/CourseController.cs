using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Course_Enrollment_System.Model;
using Online_Course_Enrollment_System.Services;

namespace Online_Course_Enrollment_System.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseManagementService courseManagementService;

        public CourseController(ICourseManagementService cm)
        {
            this.courseManagementService = cm;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = courseManagementService.GetAll();
            return Ok(courses);
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            courseManagementService.AddCourse(course);
            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, Course course)
        {
            if (id != course.Id)
                return BadRequest("ID mismatch.");

            var updated = courseManagementService.UpdateCourse(id, course);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            if (!courseManagementService.DeleteCourse(id))
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
