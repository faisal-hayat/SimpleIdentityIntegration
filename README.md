# Identity in ASP.NET Core 
[source](https://www.youtube.com/watch?v=wzaoQiS_9dI)

--- ---

## Libraries

- MicrosoftEntityFramework Core
- MicrosoftEntityFramework Tools
- MicrosoftEntityFramework Sql Server

--- ---

## Application Db Context

- We have used two db contexts
- as shown in code below

```C#
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityProject.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // Add custom user model attributes to override the default one
    [PersonalData]
    [Column(TypeName="nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

}
```

```C#
using IdentityProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityProject.Data;

public class IdentityProjectContext : IdentityDbContext<ApplicationUser>
{
    public IdentityProjectContext(DbContextOptions<IdentityProjectContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}

```

--- ---

## Progarm.cs Code

```C#
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityProject.Data;
using IdentityProject.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityProjectContextConnection' not found.");

builder.Services.AddDbContext<IdentityProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<IdentityProject.DataConnection.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityProjectContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
#region configure identity 
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase= false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric= false;
    options.Password.RequiredLength = 2;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.SignIn.RequireConfirmedEmail = false;
});
#endregion

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();

```

--- ---

## Applying Migration & Updating Databases


```shell
add-migration "table has been added to database" -context IdentityProjectContext
update-database  -context IdentityProjectContext
```

--- ---