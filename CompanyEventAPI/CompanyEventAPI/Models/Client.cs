using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyEventAPI.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Empresa { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string NameCEO { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string SurnameCEO { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int NombreAssistents { get; set; }

        public DateTime DataRegistre { get; private set; } = DateTime.UtcNow;

        public bool Vip => NombreAssistents >= 10;
    }
}
