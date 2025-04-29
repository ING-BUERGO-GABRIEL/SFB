var builder = WebApplication.CreateBuilder(args);

// Agrega servicios
builder.Services.AddControllers(); // <-- Cambia "AddControllersWithViews()" por "AddControllers()" ya que no usarás vistas Razor.

var app = builder.Build();

// Configura el manejo de errores y HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Sirve los archivos estáticos (frontend Vue)
app.UseDefaultFiles();   // <<--- Esta línea es nueva: sirve el index.html automáticamente
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Aquí NO debes usar MapControllerRoute para "Home/Index" porque ya no usarás vistas.
// SOLO mapeas los controladores (para tus APIs)
app.MapControllers();

// Esta línea es MUY importante para aplicaciones SPA (Vue)
app.MapFallbackToFile("index.html"); // <<--- Redirige cualquier ruta desconocida a tu index.html

app.Run();
