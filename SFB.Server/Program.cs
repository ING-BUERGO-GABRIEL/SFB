var builder = WebApplication.CreateBuilder(args);

// Agrega servicios
builder.Services.AddControllers(); // <-- Cambia "AddControllersWithViews()" por "AddControllers()" ya que no usar�s vistas Razor.

var app = builder.Build();

// Configura el manejo de errores y HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Sirve los archivos est�ticos (frontend Vue)
app.UseDefaultFiles();   // <<--- Esta l�nea es nueva: sirve el index.html autom�ticamente
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Aqu� NO debes usar MapControllerRoute para "Home/Index" porque ya no usar�s vistas.
// SOLO mapeas los controladores (para tus APIs)
app.MapControllers();

// Esta l�nea es MUY importante para aplicaciones SPA (Vue)
app.MapFallbackToFile("index.html"); // <<--- Redirige cualquier ruta desconocida a tu index.html

app.Run();
