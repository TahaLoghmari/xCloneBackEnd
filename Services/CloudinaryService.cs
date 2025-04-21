using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

public class CloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config , ILogger<CloudinaryService> logger)
    {
        logger.LogInformation($"Cloudinary Config: CloudName={config.Value.CloudName?.Length > 0}, ApiKey={config.Value.ApiKey?.Length > 0}, ApiSecret={config.Value.ApiSecret?.Length > 0}");
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Crop("limit").Width(1000).Height(1000),
            Folder = "your-folder"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return uploadResult;
    }
}
