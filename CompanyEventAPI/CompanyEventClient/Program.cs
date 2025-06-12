using Microsoft.AspNetCore.Authentication.Cookies;

namespace CompanyEventClient
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Fem esperar la construccio de la app per a que primer s'executi la API.
            await Task.Delay(3000);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // Accés a la configuració del fitxer appsettings.json
            string apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL not found");

            //Afegim Servei de connexió HttpClient
            //ApiFilms es el nom que li donem a la connexio de l'Api
            builder.Services.AddHttpClient("CompanyEventAPI", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";
            });
            builder.Services.AddSession();

            
            /*--------------*/
            var app = builder.Build();
            /*--------------*/


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //Activem les Sessions abans de l'enroutament
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}