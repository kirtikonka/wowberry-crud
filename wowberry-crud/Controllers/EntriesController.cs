using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wowberry_crud.Data;
using wowberry_crud.Models;

//Created Using Scaffolding with Entity Framework
//View are generated automatically

namespace wowberry_crud.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.entries != null ? 
                          View(await _context.entries.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.entries'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.entries == null)
            {
                return NotFound();
            }

            var entries = await _context.entries
                .FirstOrDefaultAsync(m => m.EntryID == id);
            if (entries == null)
            {
                return NotFound();
            }

            return View(entries);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntryID,Account,Narration,Currency,Credit,Debit,CreatedAt")] Entries entries)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entries);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entries);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.entries == null)
            {
                return NotFound();
            }

            var entries = await _context.entries.FindAsync(id);
            if (entries == null)
            {
                return NotFound();
            }
            return View(entries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EntryID,Account,Narration,Currency,Credit,Debit,CreatedAt")] Entries entries)
        {
            if (id != entries.EntryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entries);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntriesExists(entries.EntryID))
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
            return View(entries);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.entries == null)
            {
                return NotFound();
            }

            var entries = await _context.entries
                .FirstOrDefaultAsync(m => m.EntryID == id);
            if (entries == null)
            {
                return NotFound();
            }

            return View(entries);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.entries == null)
            {
                return Problem("Entity set 'ApplicationDbContext.entries'  is null.");
            }
            var entries = await _context.entries.FindAsync(id);
            if (entries != null)
            {
                _context.entries.Remove(entries);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntriesExists(int id)
        {
          return (_context.entries?.Any(e => e.EntryID == id)).GetValueOrDefault();
        }
    }
}
