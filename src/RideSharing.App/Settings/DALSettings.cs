namespace RideSharing.App.Settings;

public class DALSettings
{
    public string? ConnectionString { get; set; }
    public bool SkipMigrationAndSeedDemoData { get; set; }
}
