using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CompanyEventClient.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEventClient.Pages
{
    public class EditClientModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public EditClientModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ClientEditDTO Client { get; set; } = new ClientEditDTO();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var clientAPI = _httpClientFactory.CreateClient("CompanyEventAPI");

            var response = await clientAPI.GetAsync($"/api/Clients/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("No s'ha pogut carregar el client amb id {Id}", id);
                return RedirectToPage("/Error");
            }

            var json = await response.Content.ReadAsStringAsync();
            var clientGetDTO = JsonSerializer.Deserialize<ClientEditDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (clientGetDTO != null)
            {
                Client = clientGetDTO;
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("AuthToken");
            if (!Tools.TokenHelper.IsTokenSession(token)) return RedirectToPage("/Login");

            var clientAPI = _httpClientFactory.CreateClient("CompanyEventAPI");
            clientAPI.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonSerializer.Serialize(Client), Encoding.UTF8, "application/json");

            var response = await clientAPI.PutAsync($"/api/Clients/put/{Client.Id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error en actualitzar el client amb id {Id}", Client.Id);
                return Page(); // mostra la pàgina amb errors
            }

            return RedirectToPage("Index");
        }
    }
}
