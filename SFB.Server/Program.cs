using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Helper;
using SFB.Infrastructure.Models;
using SFB.Server.Shared;
using SFB.Shared.Backend.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

var SFB = builder.Configuration.GetSection("ConnectionString:SFB").Get<DBConfiguration>();
builder.Services.AddDbContext<SFBContext>(options =>
{
    EFProvidersHelper.UseConfiguredProviderName(options, SFB);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
        //.WithOrigins("http://localhost:5173")
          .AllowAnyOrigin()    // acepta cualquier dominio
          .AllowAnyMethod()    // GET, POST, PUT, DELETE, OPTIONS…
          .AllowAnyHeader();   // Content-Type, Authorization…n
    });
});

builder.Services.AddEndpointsApiExplorer();

var modules = CustomModules.GetBackendModules();
ModuleLoader.LoadBackendModules(modules, builder.Services);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();   // <<--- Esta l�nea es nueva: sirve el index.html autom�ticamente
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html"); 
app.Run();
