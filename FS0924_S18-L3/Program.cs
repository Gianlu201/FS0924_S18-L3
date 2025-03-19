using FS0924_S18_L3.Data;
using FS0924_S18_L3.Models;
using FS0924_S18_L3.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Recupera la stringa di connessione dal file di configurazione (appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configura il contesto del database con MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Configurazione di Identity con utenti e ruoli personalizzati
builder
    .Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        // Imposta se l'account deve essere confermato via email prima di poter accedere
        options.SignIn.RequireConfirmedAccount = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireConfirmedAccount");

        // Imposta la lunghezza minima della password
        options.Password.RequiredLength = builder
            .Configuration.GetSection("Identity")
            .GetValue<int>("RequiredLength");

        // Richiede che la password contenga almeno un numero
        options.Password.RequireDigit = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireDigit");

        // Richiede almeno una lettera minuscola nella password
        options.Password.RequireLowercase = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireLowercase");

        // Richiede almeno un carattere speciale nella password
        options.Password.RequireNonAlphanumeric = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireNonAlphanumeric");

        // Richiede almeno una lettera maiuscola nella password
        options.Password.RequireUppercase = builder
            .Configuration.GetSection("Identity")
            .GetValue<bool>("RequireUppercase");
    })
    // Utilizza il contesto del database per archiviare utenti e ruoli
    .AddEntityFrameworkStores<ApplicationDbContext>()
    // Aggiunge provider di token predefiniti per la gestione delle autenticazioni e conferme
    .AddDefaultTokenProviders();

//CONFIGURAZIONE DELL'AUTENTICAZIONE CON I COOKIE
// Le proprietà DefaultAuthenticateScheme e DefaultChallengeScheme vengono utilizzate per definire
// come il sistema di autenticazione gestisce le richieste e le sfide di autenticazione
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Schema di autenticazione predefinito
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Schema per le sfide di autenticazione
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Percorso della pagina di login
        options.AccessDeniedPath = "/Account/Login"; // Pagina di accesso negato
        options.Cookie.HttpOnly = true; // Impedisce l'accesso ai cookie tramite JavaScript per motivi di sicurezza
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Durata della sessione di autenticazione
        options.Cookie.Name = "EcommerceLiveEfCore"; // Nome del cookie per l'autenticazione
    });

builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<LoggerService>();

// REGISTRAZIONE DEI SERVIZI PERSONALIZZATI NEL CONTAINER DI DEPENDENCY INJECTION
builder.Services.AddScoped<UserManager<ApplicationUser>>(); // Servizio per la gestione degli utenti
builder.Services.AddScoped<SignInManager<ApplicationUser>>(); // Servizio per la gestione dell'accesso degli utenti
builder.Services.AddScoped<RoleManager<ApplicationRole>>(); // Servizio per la gestione dei ruoli

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

// Abilita il middleware per la gestione dell'autenticazione degli utenti
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
