namespace CompanyEventAPI.DTO
{
    public class RegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
    }
}
