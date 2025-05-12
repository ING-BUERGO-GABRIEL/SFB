using SFB.Server.Shared;
using SFB.Shared.Backend.Helpers;

var builder = WebApplication.CreateBuilder(args);

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
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html"); 
app.Run();
