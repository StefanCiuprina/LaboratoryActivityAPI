using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LaboratoryActivityAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace LaboratoryActivityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        private UserManager<ApplicationUser> _userManager;

        public StudentController(AuthenticationContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentModel>>> GetStudent()
        {
            return await _context.Student.ToListAsync();
        }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentModel>> GetStudentModel(string id)
        {
            var studentModel = await _context.Student.FindAsync(id);

            if (studentModel == null)
            {
                return NotFound();
            }

            return studentModel;
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentModel(string id, StudentModel studentModel)
        {
            if (id != studentModel.StudentId)
            {
                return BadRequest();
            }

            _context.Entry(studentModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentModel>> PostStudentModel(StudentModel studentModel)
        {
            var user = await _userManager.FindByIdAsync(studentModel.StudentId);
            var student = new StudentModel
            {
                GroupId = studentModel.GroupId,
                Hobby = studentModel.Hobby,
                Token = studentModel.Token,
                User = user,
            };

            _context.Student.Add(student);
            try
            {
                await _context.SaveChangesAsync();
                user.Student = student;
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateException)
            {
                if (StudentModelExists(studentModel.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentModel", new { id = student.StudentId }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentModel(string id)
        {
            var studentModel = await _context.Student.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound();
            }

            _context.Student.Remove(studentModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentModelExists(string id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
