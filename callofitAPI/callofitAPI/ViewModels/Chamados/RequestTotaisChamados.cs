using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.Chamados
{
    public class RequestTotaisChamados
    {
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int usuario_id { get; set; }
    }
}