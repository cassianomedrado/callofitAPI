using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public DateTime data_criacao { get; set; }
        [Required]
        public string nome { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public int tipo_usuario_id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string senha { get; set; }
        [Required]
        public bool status { get; set; }
    }
}
