using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TreineMais.Data;
using TreineMais.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC + Razor Pages (Identity UI)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // (opcional) regras de senha padrão ficam aqui se quiser
        // options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Cookie / Redirects
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    // ✅ Mais correto do que OnRedirectToReturnUrl
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.Redirect(options.LoginPath);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.Redirect(options.AccessDeniedPath);
        return Task.CompletedTask;
    };
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Rotas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ✅ SeedData com proteção (não derruba app se banco falhar no deploy)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        await SeedData.Inicializar(services);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao executar SeedData (startup)");
        // Não dá throw pra não matar o app em produção
    }
}

app.Run();