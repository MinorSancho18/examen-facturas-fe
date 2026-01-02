using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Configuration;
using Frontend.Infrastructure.Http;
using Frontend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// API Settings
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>() 
    ?? new ApiSettings { BaseUrl = "https://localhost:7001" };
builder.Services.AddSingleton(apiSettings);

// HttpClient
builder.Services.AddHttpClient<HttpJsonClient>(client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Services
builder.Services.AddScoped<IClientesApiService, ClientesApiService>();
builder.Services.AddScoped<IProductosApiService, ProductosApiService>();
builder.Services.AddScoped<IFacturasApiService, FacturasApiService>();
builder.Services.AddScoped<ILineasFacturaApiService, LineasFacturaApiService>();
builder.Services.AddScoped<ITiposApiService, TiposApiService>();
builder.Services.AddScoped<IEstadosFacturaApiService, EstadosFacturaApiService>();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
