using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using WebPhongKham.Models;
using Microsoft.EntityFrameworkCore;

namespace WebPhongKham.Extension
{
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        //private readonly string _requiredRole;

        //public SessionAuthorizeAttribute() { }
        //public SessionAuthorizeAttribute(string requiredRole)
        //{
        //    _requiredRole = requiredRole;
        //}


        //phương thức này được gọi trước khi action đươc thực hiện
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Kiểm tra xem Session có lưu thông tin người dùng hay không
            var userName = context.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName))
            {
                // Nếu không có, chuyển hướng đến trang đăng nhập
                context.HttpContext.Session.SetString("ErrorLogin", "Bạn cần đăng nhập để truy cập trang này");
                context.Result = new RedirectToActionResult("Login", "User", new { area = "Admin" });
                return; //thoát khỏi 
            }
            //check quyền của tài khoản

            // Kiểm tra quyền của tài khoản
            //using (var scope = context.HttpContext.RequestServices.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    var account = dbContext.Accounts.Include(a => a.AccountRoles).ThenInclude(ar => ar.Role)
            //                                     .FirstOrDefault(a => a.TenTaiKhoan == userName);

            //    if (account == null || !account.AccountRoles.Any(ar => ar.Role.RoleName == _requiredRole))
            //    {
            //        context.Result = new ForbidResult();
            //    }
            //}



        }

    }
}
