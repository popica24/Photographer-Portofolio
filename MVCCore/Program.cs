using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCCore.Data;
using MVCCore.Models;
using MVCCore.Options;
using MVCCore.Services.Abstract;
using MVCCore.Services.Concrete;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("Azure"));

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Smtp"));

builder.Services.Configure<AzureContainers>(builder.Configuration.GetSection("AzurePaths"));

builder.Services.AddScoped<IRepo<StatsModel>, StatsRepo>();

builder.Services.AddScoped<IRepo<ReviewModel>, ReviewRepo>();

builder.Services.AddScoped<IRepo<AlbumModel>, AlbumRepo>();

builder.Services.AddScoped<IRepo<CoverModel>,CoverRepo>();

builder.Services.AddScoped<IExtendedAlbumOptions, AlbumRepo>();

builder.Services.AddTransient<IEmailHelper,EmailHelper>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["Azure:ConnectionString:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["Azure:ConnectionString:queue"], preferMsi: true);
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedDatabase(services);
}
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
