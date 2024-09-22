namespace WebService.Configuration;

public class DatabaseConfiguration
{
    public string ConnectionString { get; set; }
    public bool MigrateDB { get; set; }
}