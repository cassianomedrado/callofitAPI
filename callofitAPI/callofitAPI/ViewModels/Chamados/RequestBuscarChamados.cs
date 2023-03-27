using System.ComponentModel.DataAnnotations;

namespace callofitAPI.ViewModels.Chamados
{
    public class RequestBuscarChamados
    {
        public int? usuario_id { get; set; }
        public int? status_chamado_id { get; set; }
        public int? tecnico_usuario_id { get; set; }
    }
}