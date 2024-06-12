using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SimpleASPNetMVC.Data;
using SimpleASPNetMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString(SimpleASPNetMVC.Constants.StudentsDatabaseConnectionName);
builder.Services.AddDbContext<StudentContext>(options =>
options.UseSqlServer(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = SimpleASPNetMVC.Constants.AuthCookieName;
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(SimpleASPNetMVC.Constants.AuthPolicyName, policy => policy.RequireAuthenticatedUser());
});


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();
// Ensuring the database is created and seeding two students  
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StudentContext>();
    try
    {
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
        logger.LogError(ex.Message);
        logger.LogError(ex.StackTrace);
    }
}

app.UseSwagger();
app.UseSwaggerUI();

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

app.UseAuthentication(); // adding authentication middleware 
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "account",
        pattern: "Account/{action=Login}/{id?}",
        defaults: new { controller = "Account" });
app.MapControllerRoute(
    name: "print",
        pattern: "Print/{action=Index}/{id?}",
        defaults: new { controller = "Print" });



app.Run();
