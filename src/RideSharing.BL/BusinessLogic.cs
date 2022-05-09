using System.Runtime.CompilerServices;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

[assembly: InternalsVisibleTo("RideSharing.BL.Tests")]

namespace RideSharing.BL;

public class BusinessLogic
{
    public static async Task<string> UploadImageAsync(string filePath)
    {
        // Open file
        var extension = Path.GetExtension(filePath);
        var fileStream = File.OpenRead(filePath);
        var contentType = extension switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            _ => throw new FormatException(),
        };

        // Upload content
        GoogleCredential credentials = GoogleCredential.FromJson(await File.ReadAllTextAsync(@"google-cloud-credentials.json"));
        var client = await StorageClient.CreateAsync(credentials);
        var result = await client.UploadObjectAsync("ics-ridesharing", $"Images/{DateTime.Now:yyyyMMddHHmmssffff}{extension}", contentType,
            fileStream);

        return result.MediaLink;
    }
}
