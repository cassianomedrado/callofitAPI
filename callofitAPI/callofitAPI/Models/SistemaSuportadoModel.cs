using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Models
{
    public class SistemaSuportadoModel
    {
        [Required(ErrorMessage = "O id do tipo usuário é obrigatório.")]
        public int id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatória.")]
        [StringLength(60, ErrorMessage = "Nome deve ter no máximo 20 caracteres.")]
        public string nome { get; set; }

        public DateTime data_criacao { get; set; }
    }
}