using kendo_londrina.Application.DTOs.FichaFinanceira;

namespace kendo_londrina.Infra.PDF_Generators;

/*
     A geração de PDF também está implementada como worker -> pdf-gen-worker.csproj
     Este worker consome uma fila para onde são enviadas as requisições de geração de PDF
*/

public interface IPdfGenerator
{
     byte[] FichaFinanceira(FichaFinanceiraDto dto);
}
