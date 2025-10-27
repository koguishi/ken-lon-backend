using kendo_londrina.Application.DTOs.FichaFinanceira;

namespace kendo_londrina.Infra.Storages;

public interface IFileStorage
{
    Task<FileInfoDto> FilePreSignedURL(string bucketName, string key);
    Task<FileInfoDto> UploadPdfAsync(byte[] pdf, string objectKey);
}
