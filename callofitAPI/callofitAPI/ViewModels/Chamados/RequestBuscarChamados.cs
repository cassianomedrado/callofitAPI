using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.Chamados
{
    public class RequestBuscarChamados
    {
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int usuario_id { get; set; }
        public int status_chamado_id { get; set; }
    }
}