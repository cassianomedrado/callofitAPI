using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.ViewModels
{
    public class AlterarDadosUserViewModel
    {
        [Required(ErrorMessage = "ID do usuário é obrigatório.")]
        public int id { get; set; }
        [StringLength(60, ErrorMessage = "Nome de usuário deve ter no máximo 60 caracteres.")]
        public string nome { get; set; }
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [StringLength(100, ErrorMessage = "E-mail deve ter no máximo 100 caracteres.")]
        public string email { get; set; }
        public int tipo_usuario_id { get; set; }
    }
}
