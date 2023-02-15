using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Models
{
    public class HistoricoChamadoModel
    {
        public int id { get; set; }
        public DateTime data_criacao { get; set; }

        [Required(ErrorMessage = "A Ação é obrigatória.")]
        [StringLength(60, ErrorMessage = "Ação deve ter no máximo 60 caracteres.")]
        public string acao { get; set; }

        [Required(ErrorMessage = "Id do Usuário é obrigatório.")]
        public int usuario_id { get; set; }

        [Required(ErrorMessage = "Id do Chamado é obrigatório.")]
        public int chamados_id { get; set; }
    }
}