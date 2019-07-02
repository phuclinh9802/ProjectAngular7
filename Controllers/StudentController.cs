using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegisterLogin.Models;

namespace RegisterLogin.Controllers
{
    public class StudentController : Controller
    {
        private readonly RegisterContext _context;

        public StudentController(RegisterContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<Object> Index()
        {
            //var schoolContext = _context.Student.Include(s => s.Class);
            return await _context.Student.ToListAsync();
        }

        // GET: Student/Details/5
        public async Task<Object> Details(Guid? id)
        {
           
            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Object> Create([FromBody] Student student)
        {
            student.StudentId = Guid.NewGuid();
            _context.Student.Add(student);
            await _context.SaveChangesAsync();
            
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassId", student.ClassId);
            
            return Ok(student);
        }

        // GET: Student/Edit/5
        public async Task<Object> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassId", student.ClassId);

            return student;
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<Object> Edit(Guid id, [FromBody] Student student)
        {
            var StudentEx = await _context.Student.FindAsync(id);

            StudentEx.StudentName = student.StudentName;
            StudentEx.DateOfBirth = student.DateOfBirth;
            StudentEx.Gpa = student.Gpa;
            StudentEx.Email = student.Email;
            StudentEx.ClassId = student.ClassId;

            _context.Update(StudentEx);
            await _context.SaveChangesAsync();
            ViewData["ClassId"] = new SelectList(_context.Class, "ClassId", "ClassId", student.ClassId);

            return Ok(StudentEx);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<Object> DeleteConfirmed(Guid id)
        {
            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentId == id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync(); 
            return await _context.Student.ToListAsync();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
