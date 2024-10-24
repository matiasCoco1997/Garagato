using Garagato.Data.EF;
using Garagato.Logica;
using Garagato.MVC.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    // Configuración JWT
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:7258",
        ValidAudience = "http://localhost:7258",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345superSecretKey@345"))
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Login/Index";  
    options.AccessDeniedPath = "/Login/Bienvenida";
})
.AddGoogle(googleOptions =>
{

    googleOptions.ClientId = "1095927180006-n0c3t5rh57ui6ivbg72inf30ro1me78h.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
    googleOptions.CallbackPath = "/Login/LoQueReciboDeGoogle";
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
  
});

// Agrega servicios de SignalR
builder.Services.AddSignalR();

//Midelware para modificar en tiempo real los cambios esteticos
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Registrar GaragatoDatabaseContext
builder.Services.AddDbContext<GaragatoDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar UsuarioServicio
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();

// Registrar SalaServicio
builder.Services.AddScoped<ISalaServicio, SalaServicio>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  
});



var app = builder.Build();

app.UseRouting();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<signalR>("/signalR");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Bienvenida}/{id?}");

app.Run();
