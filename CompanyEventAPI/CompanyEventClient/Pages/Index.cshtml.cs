using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using CompanyEventClient.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CompanyEventClient.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public List<ClientGetDTO> Clients { get; set; } = new List<ClientGetDTO>();

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task OnGet()
    {
        var clientAPI = _httpClientFactory.CreateClient("CompanyEventAPI");

        try
        {
            //var response = await client.GetAsync("api/Films/hi");
            var response = await clientAPI.GetAsync("api/Clients");
            //var response = await client.GetFromJsonAsAsyncEnumerable<List<Film>>("Films",);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogError("Error de carrega de dades de la llista Films");
            }
            else
            {
                //_logger.LogError(await response.Content.ReadAsStringAsync());
                var json = await response.Content.ReadAsStringAsync();
                Clients = JsonSerializer.Deserialize<List<ClientGetDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}");
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        //Agafem el token
        var token = HttpContext.Session.GetString("AuthToken");
        if (!Tools.TokenHelper.IsTokenSession(token)) return RedirectToPage("/Login");

        //Obliguem al HttpClient a enviar el token en el Header:
        var client = _httpClientFactory.CreateClient("CompanyEventAPI");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.DeleteAsync($"/api/Clients/delete/{id}");

        //En aquest cas comprobem si la resposta es BadRequest
        if (response == null || !(response.StatusCode == HttpStatusCode.BadRequest))
        {
            _logger.LogError("No s'ha eliminat l'element. BadRequest response");
        }

        return RedirectToPage();
    }
}
