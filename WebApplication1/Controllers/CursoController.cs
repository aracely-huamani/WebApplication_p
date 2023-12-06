using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CursoController : ControllerBase

    {
        private readonly DBContext _context;

        public CursoController(DBContext context)
        {
            _context = context;
        }

        private static List<Course> cursos = new List<Course>();

        // Endpoint para insertar un curso
        [HttpPost(Name = "InsertCourse")]
        public void InsertarCurso(Course request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "La solicitud no puede estar vacía");
            }

            // Aquí puedes realizar la lógica para insertar el curso en tu base de datos o en la lista de cursos
            Course nuevoCurso = new Course
            {
                IdCourse = cursos.Count + 1,
                name = request.name,
                credit = request.credit
            };

            cursos.Add(nuevoCurso);
            _context.SaveChanges();
        }

        // Endpoint para eliminar curso
        // POST: Courses/Delete/5
        [HttpDelete("{id}", Name = "DeleteCourse")]

        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Course == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                return NotFound();
            }
            _context.Entry(course).State = EntityState.Modified;
            //course.Active = false;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //endpoint para elimiar lista de curso 

        [HttpDelete(Name = "DeleteListCourse")]
        public void DeleteCourses(List<int> courseId)
        {


            


    }
}