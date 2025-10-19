using Amazon.S3;
using Amazon.S3.Model;

namespace pdf_gen_worker.Storages;

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

    public async Task UploadPdfAsync(byte[] pdf, string objectKey)
    {
        // === ENVIA PARA CLOUDFLARE R2 ===
        using var s3Client = new AmazonS3Client(_accessKeyId, _secretAccessKey, _amazonS3Config);

        var putRequest = new PutObjectRequest
        {
            InputStream = new MemoryStream(pdf), // usa o stream do PDF gerado
            Key = objectKey,
            BucketName = _fichaFinanceiraBucket,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };
        var response = await s3Client.PutObjectAsync(putRequest);
        // Console.WriteLine("ETag: {0}", response.ETag);
        // Console.WriteLine("response: {0}", response);
    }
}
