using Amazon.S3;
using Amazon.S3.Model;

namespace kendo_londrina.Infra.Storages;

public class CloudflareR2(IConfiguration config) : IFileStorage
{
    private readonly string _accessKeyId = config["CloudflareR2:AccessKeyId"]
        ?? throw new ArgumentNullException("CloudflareR2:AccessKeyId");
    private readonly string _secretAccessKey = config["CloudflareR2:SecretAccessKey"]
        ?? throw new ArgumentNullException("CloudflareR2:SecretAccessKey");
    private readonly string _fichaFinanceiraBucket = config["CloudflareR2:FichaFinanceiraBucket"]
        ?? throw new ArgumentNullException("CloudflareR2:FichaFinanceiraBucket");

    private readonly AmazonS3Config _amazonS3Config = new()
    {
        ServiceURL = $"https://{config["CloudflareR2:AccountId"]}.r2.cloudflarestorage.com",
        ForcePathStyle = true, // obrigatório no R2
    };

    public async Task<FileInfoDto> FilePreSignedURL(string bucketName, string key)
    {
        try
        {
            using var s3Client = new AmazonS3Client(_accessKeyId, _secretAccessKey, _amazonS3Config);

            var response = await s3Client.GetObjectMetadataAsync(bucketName, key);

            var preSignedUrl = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddMinutes(5),
            });

            return new FileInfoDto
            {
                Status = "pronto",
                Url = preSignedUrl
            };
        }
        catch (AmazonS3Exception ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new FileInfoDto
                {
                    Status = "nao encontrado",
                    Url = ""
                };
            // Outros erros (credenciais, rede, etc)
            throw;
        }
    }

}
