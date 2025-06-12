using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CompanyEventClient.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEventClient.Pages
{
    public class RegisterClientModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RegisterClientModel> _logger;

        public RegisterClientModel(IHttpClientFactory httpClientFactory, ILogger<RegisterClientModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public ClientRegisterDTO Client { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();


            var clientAPI = _httpClientFactory.CreateClient("CompanyEventAPI");


            // Crear usuari
            var userClient = new RegisterDTO
            {
                Email = Client.Email,
                Password = Client.Password,
                UserName = "EasterEgg",
                Name = Client.NameCEO,
                Surename = Client.SurnameCEO
            };

            var userJsonContent = new StringContent(
                JsonSerializer.Serialize(userClient),
                Encoding.UTF8,
                "application/json");

            var responseUser = await clientAPI.PostAsync("/api/Auth/client/registre", userJsonContent);

            if (!responseUser.IsSuccessStatusCode)
            {
                _logger.LogError("Error creant el user");
                return Page();
            }

            //Crear client
            var newClient = new ClientInsertDTO
            {
                Email = Client.Email,
                Empresa = Client.Empresa,
                Name = Client.NameCEO,
                Surename = Client.SurnameCEO,
                NombreAssistents = Client.NombreAssistents
            };


            var clientJsonContent = new StringContent(
                JsonSerializer.Serialize(newClient),
                Encoding.UTF8,
                "application/json");

            var responseClient = await clientAPI.PostAsync("/api/Clients", clientJsonContent);

            if (!responseClient.IsSuccessStatusCode)
            {
                _logger.LogError("Error creant el client");
                return Page();
            }


            return RedirectToPage("/Index");
        }
    }
}
