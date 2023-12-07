using JustCare_MB;
using JustCare_MB.Data;
using JustCare_MB.Services;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IUsersService,UsersService>();
builder.Services.AddDbContext<JustCareContext>(options =>
        options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=JustCare3;Trusted_Connection=True;"));
builder.Services.AddScoped<UsersService>(); // Assuming UsersService has Scoped lifetime
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentBookedService, AppointmentBookedService>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
