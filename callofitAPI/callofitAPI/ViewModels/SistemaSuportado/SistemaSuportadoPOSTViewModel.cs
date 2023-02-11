using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.SistemaSuportado
{
    public class SistemaSuportadoPOSTViewModel
    {
        [Required(ErrorMessage = "O Nome é obrigatória.")]
        [StringLength(60, ErrorMessage = "Nome deve ter no máximo 20 caracteres.")]
        public string nome { get; set; }
    }
}
