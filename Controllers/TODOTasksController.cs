using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODO.Entities;

namespace TODO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TODOTasksController : ControllerBase
    {
        private readonly TODOContext _context;

        public TODOTasksController(TODOContext context)
        {
            _context = context;
        }

        // GET: api/TODOTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TODOTask>>> GetTodoTask()
        {
            return await _context.TodoTask.ToListAsync();
        }

        // GET: api/TODOTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TODOTask>> GetTODOTask(int id)
        {
            var tODOTask = await _context.TodoTask.FindAsync(id);

            if (tODOTask == null)
            {
                return NotFound();
            }

            return tODOTask;
        }

        // PUT: api/TODOTasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTODOTask(int id, TODOTask tODOTask)
        {
            if (id != tODOTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(tODOTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TODOTaskExists(id))
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

        // POST: api/TODOTasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TODOTask>> PostTODOTask(TODOTask tODOTask)
        {
            _context.TodoTask.Add(tODOTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTODOTask", new { id = tODOTask.Id }, tODOTask);
        }

        // DELETE: api/TODOTasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TODOTask>> DeleteTODOTask(int id)
        {
            var tODOTask = await _context.TodoTask.FindAsync(id);
            if (tODOTask == null)
            {
                return NotFound();
            }

            _context.TodoTask.Remove(tODOTask);
            await _context.SaveChangesAsync();

            return tODOTask;
        }

        private bool TODOTaskExists(int id)
        {
            return _context.TodoTask.Any(e => e.Id == id);
        }
    }
}
