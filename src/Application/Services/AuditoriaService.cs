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
            string entidadeNome,
            string acao,
            T? entidade,
            object? dadosAntes = null)
        {
            Guid? entidadeId = null;
            if (entidade != null)
            {
                // Tenta pegar uma propriedade "Id" da entidade
                var entidadeIdProp = typeof(T).GetProperty("Id");
                entidadeId = entidadeIdProp != null ? (Guid)entidadeIdProp.GetValue(entidade)! : Guid.NewGuid();
            }

            var log = new AuditoriaEntry
            {
                Entidade = entidadeNome, // entidadeType,
                EntidadeId = entidadeId,
                Acao = acao,
                DadosAntes = dadosAntes == null
                    ? null
                    : JsonConvert.SerializeObject(dadosAntes),
                DadosDepois = entidade == null
                    ? null
                    : JsonConvert.SerializeObject(entidade),
                Data = DateTime.UtcNow,
                UsuarioId = Guid.Parse(_currentUser.UserId),
                EmpresaId = Guid.Parse(_currentUser.EmpresaId!)
            };

            try
            {
                await _repoAuditoria.AdicionarAsync(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
