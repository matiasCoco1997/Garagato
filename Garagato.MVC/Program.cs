using Garagato.MVC.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios de SignalR
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
// Install-Package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation (herramientas -> Herramientas -> Administrador de paquetes NuGet -> Consola del Administrador de paquetes )


var app = builder.Build();

// Configura el middleware de SignalR
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub");
});

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
    pattern: "{controller=Login}/{action=Bienvenida}/{id?}");

app.Run();
