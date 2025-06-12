using System.ComponentModel.DataAnnotations;

namespace CompanyEventClient.DTO
{
    public class ClientRegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Empresa { get; set; }
        public string NameCEO { get; set; }
        public string SurnameCEO { get; set; }
        public int NombreAssistents { get; set; }
    }
}
