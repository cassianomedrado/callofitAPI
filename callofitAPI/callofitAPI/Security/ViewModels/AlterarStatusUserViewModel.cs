using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.ViewModels
{
    public class AlterarStatusUserViewModel
    {
        [Required(ErrorMessage = "O id do usuário é obrigatório.")]
        public int id { get; set; }
        public bool status { get; set; }
    }
}
