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

        public readonly string[] _requerieRoles;

        //khi dùng params với arr, c# sẽ tự định nghĩa các đối số truyền vào thành kiểu arr
        //nên không cần phải tạo mảng rồi truyền vào 
        public SessionAuthorizeAttribute(params string[] roles)
        {
            _requerieRoles = roles;
        }

        //phương thức này được gọi trước khi action đươc thực hiện
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;
            var userName = session.GetString("UserName");
            // Kiểm tra xem Session có lưu thông tin người dùng hay không
            if (string.IsNullOrEmpty(userName))
            {
                // Nếu không có, chuyển hướng đến trang đăng nhập
                session.SetString("ErrorLogin", "Bạn cần đăng nhập để truy cập trang này");
                context.Result = new RedirectToActionResult("Login", "User", new { area = "Admin" });
                return; 
            }
            //kiểm tra quyền nếu có 
            if (_requerieRoles.Length > 0 && _requerieRoles != null)
            {
                //lấy thông tin Roles từ session
                var rolesString = session.GetString("Roles");
                if (string.IsNullOrEmpty(rolesString))
                {
                    //ForbidResult là 1 class đại diện cho từ chối truy cập
                    context.Result = new ForbidResult();
                    return;
                }
                //tách chuỗi roles từ session thành 1 arr
                var userRoles = rolesString.Split(',');
                bool hasRequerieRole = _requerieRoles.Any(role => userRoles.Contains(role));
                if (!hasRequerieRole)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }

    }
}
