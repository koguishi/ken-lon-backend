using kendo_londrina.Infra.Auditoria;
using Newtonsoft.Json;

namespace kendo_londrina.Application.Services
{
    public class AuditoriaService(IAuditoriaRepository repoAuditoria
        , ICurrentUserService currentUser)
    {
        private readonly IAuditoriaRepository _repoAuditoria = repoAuditoria;
        private readonly ICurrentUserService _currentUser = currentUser;

        public async Task LogAsync<T>(
            T entidade,
            string acao,
            object? dadosAntes = null)
        {
            if (entidade == null) throw new ArgumentNullException(nameof(entidade));

            var entidadeType = typeof(T).Name;

            // Tenta pegar uma propriedade "Id" da entidade
            var entidadeIdProp = typeof(T).GetProperty("Id");
            var entidadeId = entidadeIdProp != null ? (Guid)entidadeIdProp.GetValue(entidade)! : Guid.NewGuid();

            try
            {
                var jsonString = JsonConvert.SerializeObject(entidade);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var log = new AuditoriaEntry
            {
                Entidade = entidadeType,
                EntidadeId = entidadeId,
                Acao = acao,
                DadosAntes = dadosAntes == null
                    ? null
                    : JsonConvert.SerializeObject(dadosAntes),
                DadosDepois = JsonConvert.SerializeObject(entidade),
                Data = DateTime.UtcNow,
                UsuarioId = Guid.Parse(_currentUser.UserId),
                EmpresaId = Guid.Parse(_currentUser.EmpresaId!)
            };

            await _repoAuditoria.AdicionarAsync(log);
        }
    }
}
