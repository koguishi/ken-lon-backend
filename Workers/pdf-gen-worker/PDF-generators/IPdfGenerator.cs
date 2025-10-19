namespace pdf_gen_worker.PDF_Generators;

public interface IPdfGenerator
{
     byte[] FichaFinanceira(FichaFinanceiraDto dto);
}
