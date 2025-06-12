namespace CompanyEventAPI.DTO
{
    public class ClientGetDTO
    {
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string NameCEO { get; set; }
        public string SurnameCEO { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int NombreAssistents { get; set; }
        public bool Vip { get; set; }
        public DateTime DataRegistre { get; set; }
    }
}
