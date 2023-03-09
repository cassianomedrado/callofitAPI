using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.HistoricoChamado
{
    public class HistoricoChamadoViewModel
    {

        [Required(ErrorMessage = "A Ação é obrigatória.")]
        [StringLength(60, ErrorMessage = "Ação deve ter no máximo 60 caracteres.")]
        public string acao { get; set; }

        [Required(ErrorMessage = "Id do Usuário é obrigatório.")]
        public int usuario_id { get; set; }

        [Required(ErrorMessage = "Id do Chamado é obrigatório.")]
        public int chamados_id { get; set; }

    }
}