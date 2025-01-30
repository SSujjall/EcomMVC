using AspNetCoreHero.ToastNotification;
using CloudinaryDotNet;
using EcomSiteMVC.Core.IRepositories;
using EcomSiteMVC.Core.IServices;
using EcomSiteMVC.Core.Models.Configs;
using EcomSiteMVC.Extensions.EmailService.Config;
using EcomSiteMVC.Extensions.EmailService.Service;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Config;
using EcomSiteMVC.Extensions.KhaltiPaymentService.Service;
using EcomSiteMVC.Infrastructure.Data.Contexts;
using EcomSiteMVC.Infrastructure.Repositories;
using EcomSiteMVC.Infrastructure.Services;
using EcomSiteMVC.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EcomDB")).UseLazyLoadingProxies();
});

// Configure Cloudinary
var cloudinaryConfig = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinaryConfig>();
var account = new Account(cloudinaryConfig.CloudName, cloudinaryConfig.ApiKey, cloudinaryConfig.ApiSecret);
var cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);

// Email Configuration
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);

// Khalti Configuration
var khaltiConfig = builder.Configuration.GetSection("Payment:Khalti").Get<KhaltiConfig>();
builder.Services.AddSingleton(khaltiConfig);

// Configuring Authentications properties
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
  // For users who ticks the "remember me?" checkbox during login 
  .AddCookie(options =>
  {
      options.LoginPath = "/Auth/LoginView"; // if not authorized, send to login page
      options.LogoutPath = "/Auth/Logout";
      options.AccessDeniedPath = "/NotFound";
  }).AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
  {
      googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
      googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
      googleOptions.SaveTokens = true;
  });

// For users who login but do not ticks the "remember me?" checkbox
// (The login session is deleted when the browser is closed or if inactive for 30 mins)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout 
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "EcomSiteMVCSession";
});

// notification services
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.BottomRight;
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();


// Register generic/helper repositories and classes
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IEmailService, EmailService>();

//builder.Services.AddTransient<TokenHelper>(); // Implement this later for jwt authentication instead of identity

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Register generic/helper repositories and classes
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IKhaltiService, KhaltiService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();


// Cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});


// Custom View Expander for nested Views folder
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
    });

// Adding HttpContextAccessor for accessing Request Context in Services Classes (OrderService.cs)
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient(); // Adding HTTP Client FOR CALLING APIs

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirect to "PageNotFound" view if the endpoint is invalid
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/NotFound";
        await next();
    }
    //// UNCOMMENT THIS WHEN YOU DONT WANT TO SEE EXCEPTION PAGE if exception occurs
    //else
    //{
    //    context.Request.Path = "/Home/Error";
    //    await next();
    //}
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
