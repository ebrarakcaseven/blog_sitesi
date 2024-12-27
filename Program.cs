using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Blog.Identity; // IdentityDataContext ve IdentityInitializer için gerekli namespace
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DefaultConnection için DataContext'i ekliyoruz
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// IdentityConnection için IdentityDataContext'i ekliyoruz
builder.Services.AddDbContext<IdentityDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

// Identity işlemleri için gerekli servisleri ekliyoruz
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        // Şifre politikalarını yapılandırıyoruz
        options.Password.RequireDigit = false; // Rakam zorunluluğu kapalı
        options.Password.RequireNonAlphanumeric = false; // Özel karakter zorunluluğu kapalı
        options.Password.RequiredLength = 5; // Minimum şifre uzunluğu
        options.Password.RequireUppercase = false; // Büyük harf zorunluluğu kapalı
        options.Password.RequireLowercase = false; // Küçük harf zorunluluğu kapalı
    })
    .AddEntityFrameworkStores<IdentityDataContext>() // IdentityDataContext kullanılıyor
    .AddDefaultTokenProviders();

// Kimlik doğrulama için çerez yapılandırması
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriş sayfası yolu
        options.AccessDeniedPath = "/Account/AccessDenied"; // Erişim reddedildiğinde yönlendirme
        options.LogoutPath = "/Account/LogOut"; // Çıkış yapma sayfası yolu
    });

var app = builder.Build();

// Veritabanı otomatik migration işlemleri ve IdentityInitializer entegrasyonu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // DefaultConnection için migration
        var dataContext = services.GetRequiredService<DataContext>();
        dataContext.Database.Migrate();

        // IdentityConnection için migration
        var identityContext = services.GetRequiredService<IdentityDataContext>();
        identityContext.Database.Migrate();

        // IdentityInitializer kullanılarak roller ve kullanıcılar oluşturuluyor
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        await IdentityInitializer.Initialize(services);  // Burada Initialize metodunu çağırıyoruz
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı başlatılırken hata oluştu.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization();  // Yetkilendirme

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
