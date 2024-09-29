using AppBlogUdeM.AccesoDatos.Data.Repositorio;
using AppBlogUdeM.AccesoDatos.Data.Repositorio.IRepositorio;
using AppBlogUdeM.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


//Agregar contenedor de trabajo al contenedor IOC

builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Usuarios}/{controller=Home}/{action=Index}/{id?}"); //Se agrega el area del cliente , ahi esta el home controller 
app.MapRazorPages();                                                    //el punto de entra de la aplicacion

app.Run();
