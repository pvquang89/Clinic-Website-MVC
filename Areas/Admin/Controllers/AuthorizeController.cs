using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPhongKham.Extension;
using WebPhongKham.Models;

namespace WebPhongKham.Areas.Admin.Controllers
{
    [Area("Admin")]
    [SessionAuthorize("Manager")] 
    public class AuthorizeController : Controller
    {
        public readonly AppDbContext _context;
        public AuthorizeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var listAcc = _context.Accounts.Include(a => a.AccountRoles)
                                            .ThenInclude(ar=>ar.Role)
                                            .ToList();
            return View(listAcc);
        }
    }
}
