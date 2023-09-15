using System.ComponentModel.DataAnnotations.Schema;

namespace ApiKeyBackend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string? ApiCors { get; set; }
        public bool Cors { get; set; }
        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        public string? CorsPolicy { get; set; }
    }
}
