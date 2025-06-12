namespace CompanyEventClient.DTO
{
    public class ClientEditDTO
    {
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string NameCEO { get; set; }
        public string SurnameCEO { get; set; }
        public string Email { get; set; }
        public int NombreAssistents { get; set; }
    }
}