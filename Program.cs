using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



//Ensure db is created
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SchoolContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex) { 
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error ocurred while initializing the db.");
    }
}


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
