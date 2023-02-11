using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Service.Operacao;
using Service.Usuario;
using Service.UsuarioToken;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IUsuario, Usuario>();
builder.Services.AddSingleton<IUsuarioToken, UsuarioToken>();
builder.Services.AddSingleton<IOperacao, Operacao>();
builder.Services.AddSession();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


var culturaBrasil = new CultureInfo("pt-BR");
culturaBrasil.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
culturaBrasil.DateTimeFormat.ShortTimePattern = "HH:mm";
culturaBrasil.NumberFormat.NumberDecimalDigits = 2;
culturaBrasil.NumberFormat.NumberGroupSeparator = "_";
culturaBrasil.NumberFormat.NumberDecimalSeparator = ",";
System.Console.WriteLine(string.Format(culturaBrasil, "{0:N}", 43239.11));


app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(culturaBrasil),
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapBlazorHub();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
