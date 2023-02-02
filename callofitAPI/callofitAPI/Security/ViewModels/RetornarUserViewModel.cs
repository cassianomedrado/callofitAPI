using System.ComponentModel.DataAnnotations;

namespace callofitAPI.Security.ViewModels
{
    public class RetornarUserViewModel { 
        public int id { get; set; }
        public DateTime data_criacao { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int tipo_usuario_id { get; set; }
        public string username { get; set; }
        public bool status { get; set; }
    }
}
