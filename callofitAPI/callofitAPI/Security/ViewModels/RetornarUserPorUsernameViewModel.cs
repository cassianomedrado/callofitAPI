using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.ViewModels
{
    public class RetornarUserPorUIdViewModel
    {
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int id { get; set; }
    }
}
