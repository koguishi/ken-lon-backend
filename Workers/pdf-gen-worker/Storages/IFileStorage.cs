namespace pdf_gen_worker.Storages;

public interface IFileStorage
{
    Task UploadPdfAsync(
        byte[] pdf,
        string objectKey
    );
}
