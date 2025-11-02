using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using kendo_londrina.Application.DTOs.FichaFinanceira;

namespace kendo_londrina.Infra.PDF_Generators;
public class PdfGenerator : IPdfGenerator
{
    public byte[] FichaFinanceira(FichaFinanceiraDto dto)
    {
        using var ms = new MemoryStream();

        var writer = new PdfWriter(ms);
        writer.SetCloseStream(false);
        var pdf = new PdfDocument(writer);
        var doc = new Document(pdf);

        // ======= Estilos básicos =======
        var bold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
        var normal = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

        doc.SetFont(normal);
        doc.SetFontSize(11);

        // ======= Cabeçalho =======
        var titulo = new Paragraph($"Ficha Financeira {dto.Ano}")
            .SetFont(bold)
            .SetFontSize(16)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMarginBottom(20);
        doc.Add(titulo);

        doc.Add(
            new Paragraph()
                .Add(new Text("Nome: "))
                .Add(new Text($"{dto.NomePessoa}").SetFont(bold))
                .Add("\n")
                .Add(new Text("Vencimento: "))
                .Add(new Text($"{dto.VencimentoInicial.ToString("dd/MM/yyyy")} - {dto.VencimentoFinal.ToString("dd/MM/yyyy")}"))
        );

        // ======= Tabela de mensalidades =======
        var table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 2, 2 }))
            .UseAllAvailableWidth();

        // Cabeçalho da tabela
        var headerBg = new DeviceRgb(230, 230, 230);
        var headerStyle = new Style()
            .SetBackgroundColor(headerBg)
            .SetFont(bold)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetPadding(5);

        table.AddHeaderCell(new Cell().Add(new Paragraph("Vencimento")).AddStyle(headerStyle));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Valor")).AddStyle(headerStyle));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Pagamento")).AddStyle(headerStyle));

        // Linhas de dados
        var evenRow = new DeviceRgb(245, 245, 245); // cinza bem claro
        var oddRow = ColorConstants.WHITE;

        for (int i = 0; i < dto.Titulos.Count; i++)
        {
            var m = dto.Titulos[i];
            var pagamento = m.DataLiquidacao.HasValue
                ? m.DataLiquidacao.Value.ToString("dd/MM/yyyy")
                : "Não pago";

            var bg = (i % 2 == 0) ? evenRow : oddRow;

            table.AddCell(new Cell().Add(new Paragraph(m.Vencimento.ToString("dd/MM/yyyy")))
                .SetBackgroundColor(bg)
                .SetPadding(5));
            table.AddCell(new Cell().Add(new Paragraph($"R$ {m.Valor:N2}"))
                .SetBackgroundColor(bg)
                .SetPadding(5));
            table.AddCell(new Cell().Add(new Paragraph(pagamento))
                .SetBackgroundColor(bg)
                .SetPadding(5));
        }

        doc.Add(table);

        // ======= Resumo =======
        if (dto.Resumo is not null)
        {
            doc.Add(new Paragraph("\nResumo Financeiro")
                .SetFont(bold)
                .SetFontSize(13)
                .SetTextAlignment(TextAlignment.LEFT));

            var resumoTable = new Table(UnitValue.CreatePercentArray(new float[] { 2, 1 }))
                .UseAllAvailableWidth();

            resumoTable.AddCell("Total de Mensalidades:");
            resumoTable.AddCell($"R$ {dto.Resumo.Total:N2}");

            resumoTable.AddCell("Total Pago:");
            resumoTable.AddCell($"R$ {dto.Resumo.TotalLiquidado:N2}");

            resumoTable.AddCell("Total em Aberto:");
            resumoTable.AddCell($"R$ {dto.Resumo.TotalEmAberto:N2}");

            resumoTable.AddCell("Última Atualização:");
            resumoTable.AddCell($"{dto.Resumo.UltimaAtualizacao:dd/MM/yyyy HH:mm}");

            doc.Add(resumoTable);
        }

        // ======= Rodapé opcional =======
        doc.Add(new Paragraph("\n\nGerado automaticamente em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
            .SetFontSize(9)
            .SetTextAlignment(TextAlignment.RIGHT)
            .SetFontColor(ColorConstants.GRAY));

        doc.Close();
        ms.Position = 0;
        return ms.ToArray();
    }

}
