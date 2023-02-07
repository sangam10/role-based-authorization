using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasklist.Data;
using Tasklist.Models;

namespace Tasklist.Controllers
{
    public class TaskEntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskEntitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TaskEntities
        public async Task<IActionResult> Index()
        {
              return _context.Tasks != null ? 
                          View(await _context.Tasks.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tasks'  is null.");
        }

        // GET: TaskEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            return View(taskEntity);
        }

        // GET: TaskEntities/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaskEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CreatedAt")] TaskEntity taskEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskEntity);
        }

        // GET: TaskEntities/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity == null)
            {
                return NotFound();
            }
            return View(taskEntity);
        }

        // POST: TaskEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,CreatedAt")] TaskEntity taskEntity)
        {
            if (id != taskEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskEntityExists(taskEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskEntity);
        }

        // GET: TaskEntities/Delete/5
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            return View(taskEntity);
        }

        // POST: TaskEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tasks'  is null.");
            }
            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity != null)
            {
                _context.Tasks.Remove(taskEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskEntityExists(int id)
        {
          return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
