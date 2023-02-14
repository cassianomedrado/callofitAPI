using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Models
{
    public class StatusChamadoViewModel
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(60, ErrorMessage = "Descrição deve ter no máximo 60 caracteres.")]
        public string descricao { get; set; }
    }
}
