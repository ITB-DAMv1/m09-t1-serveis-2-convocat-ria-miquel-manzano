using System.ComponentModel.DataAnnotations;

namespace CompanyEventAPI.DTO
{
    public class ClientInsertDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Empresa { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public int NombreAssistents { get; set; }
    }
}