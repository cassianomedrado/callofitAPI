using callofitAPI.Util;

namespace callofitAPI.Interfaces
{
    public interface INotificador
    {
        public void Adicionar(Notificacao notificacao);
        public List<Notificacao> ObterNotificacoes();
    }
}
