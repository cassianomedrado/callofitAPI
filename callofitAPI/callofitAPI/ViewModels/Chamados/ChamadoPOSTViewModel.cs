using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.Chamados
{
    public class ChamadoPOSTViewModel
    {
        [Required(ErrorMessage = "O solicitante é obrigatório.")]
        [StringLength(60, ErrorMessage = "Ação deve ter no máximo 60 caracteres.")]
        public string solicitante { get; set; }

        [Required(ErrorMessage = "A data limite é obrigatório.")]
        public DateTime data_limite { get; set; }

        [Required(ErrorMessage = "Id do status do chamado é obrigatório.")]
        public int status_chamado_id { get; set; }

        [Required(ErrorMessage = "Id do sistema suprotado é obrigatório.")]
        public int sistema_suportado_id { get; set; }

        [StringLength(500, ErrorMessage = "Ação deve ter no máximo 500 caracteres.")]
        public string descricao_problema { get; set; }
        public int usuario_id { get; set; }
        public int? tecnico_usuario_id { get; set; }
        public string descricao_solucao { get; set; }

        [Required(ErrorMessage = "Id do tipo de chamado é obrigatório.")]
        public int tipo_chamado_id { get; set; }
    }
}