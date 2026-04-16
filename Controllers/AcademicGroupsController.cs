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
    public class AcademicGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicGroups
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AcademicGroups.Include(a => a.Curator);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AcademicGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicGroup = await _context.AcademicGroups
                .Include(a => a.Curator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicGroup == null)
            {
                return NotFound();
            }

            return View(academicGroup);
        }

        // GET: AcademicGroups/Create
        public IActionResult Create()
        {
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "Id", "FirstName");
            return View();
        }

        // POST: AcademicGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Specialty,Year,CuratorId")] AcademicGroup academicGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "Id", "FirstName", academicGroup.CuratorId);
            return View(academicGroup);
        }

        // GET: AcademicGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicGroup = await _context.AcademicGroups.FindAsync(id);
            if (academicGroup == null)
            {
                return NotFound();
            }
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "Id", "FirstName", academicGroup.CuratorId);
            return View(academicGroup);
        }

        // POST: AcademicGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Specialty,Year,CuratorId")] AcademicGroup academicGroup)
        {
            if (id != academicGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicGroupExists(academicGroup.Id))
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
            ViewData["CuratorId"] = new SelectList(_context.Teachers, "Id", "FirstName", academicGroup.CuratorId);
            return View(academicGroup);
        }

        // GET: AcademicGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicGroup = await _context.AcademicGroups
                .Include(a => a.Curator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicGroup == null)
            {
                return NotFound();
            }

            return View(academicGroup);
        }

        // POST: AcademicGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicGroup = await _context.AcademicGroups.FindAsync(id);
            if (academicGroup != null)
            {
                _context.AcademicGroups.Remove(academicGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicGroupExists(int id)
        {
            return _context.AcademicGroups.Any(e => e.Id == id);
        }
    }
}
