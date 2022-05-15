using System.Runtime.CompilerServices;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Directions.Request;


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

    public static async Task<Tuple<bool, int?, TimeSpan?>> GetRouteInfoAsync(string from, string to)
    {
        DirectionsRequest request = new()
        {
            Key = await File.ReadAllTextAsync(@"google-maps-credentials.txt"),
            Origin = new LocationEx(new Address(from)),
            Destination = new LocationEx(new Address(to))
        };

        var response = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
        if (response.Status != Status.Ok)
            return new Tuple<bool, int?, TimeSpan?>(false, null, null);

        var distance = response.Routes.FirstOrDefault()?.Legs.FirstOrDefault()?.Distance;
        var duration = response.Routes.FirstOrDefault()?.Legs.FirstOrDefault()?.Duration;
        if (distance is null || duration is null)
            return new Tuple<bool, int?, TimeSpan?>(false, null, null);

        return new Tuple<bool, int?, TimeSpan?>(true, distance.Value / 1000, TimeSpan.FromSeconds(duration.Value));

    }
}
