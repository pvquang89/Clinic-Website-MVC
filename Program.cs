using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebPhongKham.Extension;
using WebPhongKham.Models;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.
builder.Services.AddControllersWithViews();

//cấu hình sử dụng bộ nhớ đệm để lưu session
builder.Services.AddDistributedMemoryCache();
//cấu hình session 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập cookie từ server
    options.Cookie.IsEssential = true; // Cookie cần thiết cho ứng dụng
    options.Cookie.Name = "ClinicWebsite-Session"; // Đặt tên cho cookie session
});

//cấu hình dịch vụ xác thực
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Admin/User/Login"; // Đường dẫn tới trang đăng nhập
        options.AccessDeniedPath = "/Admin/Dashboard/NoAccess"; // Đường dẫn tới trang từ chối
    });

//Cấu hình DbContext lấy chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbClinicWebsite")));


//đăng ký DI cho fileuploadhelper 
builder.Services.AddSingleton<FileUploadHelper>();
//đăng ký DI cho session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//thêm app.UseAuthentication(); để cấu hình dịch vụ xác thực
app.UseAuthentication(); 
app.UseAuthorization();

//cấu hình session sau app.UseRouting(); 
app.UseSession(); 

//cấu hình routing cho areas
app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
