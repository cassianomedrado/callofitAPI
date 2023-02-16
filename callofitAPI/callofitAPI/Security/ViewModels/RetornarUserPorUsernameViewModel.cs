using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.ViewModels
{
    public class RetornarUserPorUsernameViewModel
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        [StringLength(60, ErrorMessage = "Username deve ter no máximo 60 caracteres.")]
        public string username { get; set; }
    }
}
