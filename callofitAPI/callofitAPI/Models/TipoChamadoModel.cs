using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Models
{
    public class TipoChamadoModel
    {
        [Required(ErrorMessage = "O id do tipo chamado é obrigatório.")]
        public int id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(60, ErrorMessage = "Descrição deve ter no máximo 60 caracteres.")]
        public string descricao { get; set; }
    }
}
