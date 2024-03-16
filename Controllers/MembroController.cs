using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNETLogin.Data;
using ASPNETLogin.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASPNETLogin.Controllers
{
    [Authorize]
    public class MembroController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembroController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Membro
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tmembros.Include(m => m.Equipa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Membro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membro = await _context.Tmembros
                .Include(m => m.Equipa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membro == null)
            {
                return NotFound();
            }

            return View(membro);
        }

        // GET: Membro/Create
        public IActionResult Create()
        {
            ViewData["EquipaId"] = new SelectList(_context.Tequipas, "Id", "Id");
            return View();
        }

        // POST: Membro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeMembro,EquipaId")] Membro membro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipaId"] = new SelectList(_context.Tequipas, "Id", "Id", membro.EquipaId);
            return View(membro);
        }

        // GET: Membro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membro = await _context.Tmembros.FindAsync(id);
            if (membro == null)
            {
                return NotFound();
            }
            ViewData["EquipaId"] = new SelectList(_context.Tequipas, "Id", "Id", membro.EquipaId);
            return View(membro);
        }

        // POST: Membro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeMembro,EquipaId")] Membro membro)
        {
            if (id != membro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembroExists(membro.Id))
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
            ViewData["EquipaId"] = new SelectList(_context.Tequipas, "Id", "Id", membro.EquipaId);
            return View(membro);
        }

        // GET: Membro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membro = await _context.Tmembros
                .Include(m => m.Equipa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (membro == null)
            {
                return NotFound();
            }

            return View(membro);
        }

        // POST: Membro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membro = await _context.Tmembros.FindAsync(id);
            if (membro != null)
            {
                _context.Tmembros.Remove(membro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembroExists(int id)
        {
            return _context.Tmembros.Any(e => e.Id == id);
        }
    }
}
