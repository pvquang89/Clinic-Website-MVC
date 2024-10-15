using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebPhongKham.Extension
{
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        //phương thức này được gọi trước khi action đươc thực hiện
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Kiểm tra xem Session có lưu thông tin người dùng hay không
            var user = context.HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(user))
            {
                // Nếu không có, chuyển hướng đến trang đăng nhập
                context.HttpContext.Session.SetString("ErrorLogin", "Bạn cần đăng nhập để truy cập trang này");
                context.Result = new RedirectToActionResult("Login", "User", new { area = "Admin" });
            }
        }

    }
}
