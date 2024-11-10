using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wowberry_crud.Data;
using wowberry_crud.Models;

namespace wowberry_crud.Controllers
{
    public class AuditController : Controller
    {
        // Inserting Private Fields
        private readonly ApplicationDbContext db;

        // Parameterised Constructor
        public AuditController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index(string tableName, DateTime? startDate, DateTime? endDate)
        {
            ViewData["TableName"] = tableName;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            var audits = db.audits.AsQueryable();

            if (!string.IsNullOrEmpty(tableName))
                audits = audits.Where(a => a.TableName == tableName);

            if (startDate.HasValue)
                audits = audits.Where(a => a.ModifiedAt >= startDate.Value);

            if (endDate.HasValue)
                audits = audits.Where(a => a.ModifiedAt <= endDate.Value);

            var auditList = await audits.OrderByDescending(a => a.ModifiedAt).ToListAsync();
            return View(auditList);
        }
    }
}
