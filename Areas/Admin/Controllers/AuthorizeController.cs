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
                                            .ThenInclude(ar => ar.Role)
                                            .ToList();
            return View(listAcc);
        }

        public IActionResult Setting(int id)
        {
            var acc = _context.Accounts.Include(a => a.AccountRoles)
                                            .ThenInclude(ar => ar.Role)
                                            .FirstOrDefault(a => a.Id == id);
            //lấy danh sách roles để hiện ở phần phân quyền 
            ViewBag.Roles = _context.Roles.ToList();
            if (acc == null)
                return NotFound();
            return View(acc);
        }

        [HttpPost]
        public IActionResult SavePermissions(int id, int[] roles)
        {
            //lấy ra acc trong csdl có id tương ứng với id gửi lên từ client
            var acc = _context.Accounts.Include(a => a.AccountRoles)
                                        .FirstOrDefault(a => a.Id == id);

            if (acc == null)
                return NotFound();

            //xoá all quyền hiện có của acc để đặt lại từ đầu
            acc.AccountRoles.Clear();

            //thêm các quyền mới dựa trên danh sách RoleId từ form
            foreach (var roleId in roles)
            {
                var role = _context.Roles.Find(roleId);
                if (role != null)
                {
                    acc.AccountRoles.Add(
                        new AccountRole
                        {
                            AccountId = role.Id,
                            RoleId = roleId
                        }
                    );
                }

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
