using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pri.Api.Music.Core.Entities;
using Pri.Api.Music.Core.Interfaces.Repositories;
using Pri.Api.Music.Core.Interfaces.Services;
using Pri.Api.Music.Core.Services;
using Pri.Api.Music.Infrastructure.Repositories;
using Pri.CleanArchitecture.Music.Core.Interfaces.Repositories;
using Pri.CleanArchitecture.Music.Core.Interfaces.Services;
using Pri.CleanArchitecture.Music.Core.Services;
using Pri.CleanArchitecture.Music.Infrastructure.Data;
using Pri.CleanArchitecture.Music.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
//Add database service
builder.Services.AddDbContext<ApplicationDbContext>
    (options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultDb")));
//register identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //ONLY FOR TESTING PURPOSES!!!!
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
//set the redirect cookies
builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/Auth/Auth/Login";
        options.LogoutPath = "/Auth/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Auth/AccessDenied";
    }
);
// Add services to the container.
builder.Services.AddScoped<IRecordRepository,RecordRepository>();
builder.Services.AddScoped<IGenreRepository,GenreRepository>();
builder.Services.AddScoped<IArtistRepository,ArtistRepository>();
builder.Services.AddScoped<IPropertyRepository,PropertyRepository>();
builder.Services.AddScoped<IRecordService,RecordService>();
builder.Services.AddScoped<IGenreService,GenreService>();
builder.Services.AddScoped<IPropertyService,PropertyService>();
builder.Services.AddScoped<IArtistService,ArtistService>();
builder.Services.AddControllersWithViews();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areaDefault",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
