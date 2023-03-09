using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.HistoricoChamado
{
    public class RequestDeleteChamado
    {
        [Required(ErrorMessage = "Id do Usuário é obrigatório.")]
        public int usuario_id { get; set; }

        [Required(ErrorMessage = "Id do Chamado é obrigatório.")]
        public int chamado_id { get; set; }

    }
}