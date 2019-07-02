using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegisterLogin.Models;

namespace RegisterLogin.Controllers
{
    public class ClassController : Controller
    {
        private readonly RegisterContext _context;

        public ClassController(RegisterContext context)
        {
            _context = context;
        }

        // GET: Class
        [HttpGet]
        public async Task<Object> Index()
        {
            return await _context.Class.ToListAsync();
        }

        // GET: Class/Details/5
        public async Task<Object> Details(Guid? ClassId)
        {
            if (ClassId == null)
            {
                return NotFound();
            }
            var getClass = await _context.Class
                .FirstOrDefaultAsync(m => m.ClassId == ClassId);
            if (getClass == null)
            {
                return NotFound();
            }

            return getClass;
        }

        // POST: Class/Create
        [HttpPost]
        public async Task<Object> Create([FromBody] Class getClass)
        {
            _context.Class.Add(getClass);
            await _context.SaveChangesAsync();
             
            return Ok(getClass);
        }

        // POST: Class/Edit/5
        [HttpPost]
        public async Task<Object> Edit(Guid ClassId, [FromBody] Class getClass)
        {
            var ClassExample = await _context.Class.FindAsync(ClassId);
            ClassExample.ClassName = getClass.ClassName;
            ClassExample.Location = getClass.Location;
            _context.Class.Update(ClassExample);
            await _context.SaveChangesAsync();

            return Ok(ClassExample);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<Object> DeleteConfirmed(Guid id)
        {
            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.ClassId == id);
            _context.Class.Remove(@class);
            await _context.SaveChangesAsync();
            
            return await _context.Class.ToListAsync();
        }

        private bool ClassExists(Guid id)
        {
            return _context.Class.Any(e => e.ClassId == id);
        }
    }
}
