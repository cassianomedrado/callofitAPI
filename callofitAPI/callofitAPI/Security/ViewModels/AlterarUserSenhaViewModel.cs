using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.ViewModels
{
    public class AlterarUserSenhaViewModel
    {
        [Required(ErrorMessage = "O nome do usuário é obrigatório.")]
        [StringLength(60, ErrorMessage = "Nome de usuário deve ter no máximo 60 caracteres.")]
        public string username { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres.")]
        public string email { get; set; }
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Senha atual deve ter de 6 a 20 caracteres.")]
        public string senhaAtual { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Nova senha deve ter de 6 a 20 caracteres.")]
        public string senhaNova { get; set; }

        [Required(ErrorMessage = "A confirmação de nova senha é obrigatória.")]
        [Compare("senhaNova", ErrorMessage = "Senhas não conferem.")]
        public string confirmaNovaSenha { get; set; }
    }
}
