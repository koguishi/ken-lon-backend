using kendo_londrina.Application.Services;
using kendo_londrina.Domain.Repositories;
using kendo_londrina.Infra.Data;
using kendo_londrina.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<KendoLondrinaContext>()
    .AddDefaultTokenProviders();

// Configura JWT
var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
var issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
var audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };

    // Habilita eventos para debug
    // options.Events = new JwtBearerEvents
    // {
    //     OnAuthenticationFailed = context =>
    //     {
    //         Console.WriteLine("Auth Failed: " + context.Exception.Message);
    //         return Task.CompletedTask;
    //     },
    //     OnTokenValidated = context =>
    //     {
    //         Console.WriteLine("Token Validado com sucesso: " +
    //             context.Principal.Identity.Name);
    //         return Task.CompletedTask;
    //     },
    //     OnChallenge = context =>
    //     {
    //         Console.WriteLine("OnChallenge: " + context.ErrorDescription);
    //         return Task.CompletedTask;
    //     }
    // };
});



builder.Services.AddAuthorization();

var frontendUrl = builder.Configuration["FRONTEND_URL"] ?? string.Empty;

// 1. Adicionar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins(frontendUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<KendoLondrinaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<PessoaService>();

builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<AlunoService>();
// builder.Services.AddScoped<IMensalidadeRepository, MensalidadeRepository>();
// builder.Services.AddScoped<MensalidadeService>();

var app = builder.Build();

// 2. Usar CORS antes de app.MapControllers()
app.UseCors("AllowReactApp");

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
