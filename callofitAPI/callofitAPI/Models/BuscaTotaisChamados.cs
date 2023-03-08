namespace callofitAPI.Models
{
    public class BuscaTotaisChamados
    {
        public int chamadosEmAberto { get; set; }
        public int chamadosPendentes { get; set; }
        public int chamadosFinalizados { get; set; }
        public int chamadosAtrasados { get; set; }
    }
}