using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DekanatUniversity.Data;
using DekanatUniversity.Models;

namespace DekanatUniversity.Controllers
{
    public class VedomostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VedomostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vedomosts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vedomosti.Include(v => v.AcademicGroup).Include(v => v.Discipline).Include(v => v.Teacher);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vedomosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vedomost = await _context.Vedomosti
                .Include(v => v.AcademicGroup)
                .Include(v => v.Discipline)
                .Include(v => v.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vedomost == null)
            {
                return NotFound();
            }

            return View(vedomost);
        }

        // GET: Vedomosts/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name");
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name");
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "UserId", "FirstName");
            return View();
        }

        // POST: Vedomosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Semester,GroupId,DisciplineId,TeacherId,DateCreated,Status")] Vedomost vedomost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vedomost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", vedomost.GroupId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name", vedomost.DisciplineId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "UserId", "FirstName", vedomost.TeacherId);
            return View(vedomost);
        }

        // GET: Vedomosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vedomost = await _context.Vedomosti.FindAsync(id);
            if (vedomost == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", vedomost.GroupId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name", vedomost.DisciplineId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "UserId", "FirstName", vedomost.TeacherId);
            return View(vedomost);
        }

        // POST: Vedomosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Semester,GroupId,DisciplineId,TeacherId,DateCreated,Status")] Vedomost vedomost)
        {
            if (id != vedomost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vedomost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VedomostExists(vedomost.Id))
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
            ViewData["GroupId"] = new SelectList(_context.AcademicGroups, "Id", "Name", vedomost.GroupId);
            ViewData["DisciplineId"] = new SelectList(_context.Disciplines, "Id", "Name", vedomost.DisciplineId);
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "UserId", "FirstName", vedomost.TeacherId);
            return View(vedomost);
        }

        // GET: Vedomosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vedomost = await _context.Vedomosti
                .Include(v => v.AcademicGroup)
                .Include(v => v.Discipline)
                .Include(v => v.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vedomost == null)
            {
                return NotFound();
            }

            return View(vedomost);
        }

        // POST: Vedomosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vedomost = await _context.Vedomosti.FindAsync(id);
            if (vedomost != null)
            {
                _context.Vedomosti.Remove(vedomost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VedomostExists(int id)
        {
            return _context.Vedomosti.Any(e => e.Id == id);
        }
    }
}
