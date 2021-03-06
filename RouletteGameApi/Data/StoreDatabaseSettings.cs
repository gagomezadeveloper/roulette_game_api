namespace RouletteGameApi.Data;

public class StoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string CollectionName { get; set; } = null!;
    public string ApiToken { get; set; } = null!;
}