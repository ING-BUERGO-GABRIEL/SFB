using SFB.Infrastructure.Contexts;
using SFB.Server.Shared;
using SFB.Shared.Backend.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure
ProgramHelper.ConfigureDBContext<SFBContext>(builder, "SFB");
ProgramHelper.ConfigureAddCors(builder, "AllowAll");
ProgramHelper.ConfigureJsonSerialize(builder);
ProgramHelper.ConfigureJwtAuthenticate(builder);

//Load Modules
var modules = CustomModules.GetBackendModules();
ModuleLoader.LoadBackendModules(modules, builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())               // <-- Solo en desarrollo
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();   
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
