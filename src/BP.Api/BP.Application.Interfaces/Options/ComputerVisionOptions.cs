namespace BP.Application.Interfaces.Options;

public class ComputerVisionOptions
{
    public string Url { get; set; } = null!;
    public string? ApiKey { get; set; } // If needed

    public string ModelVersion { get; set; } = "2023-04-15"; // Default to latest, can be overridden

    public string ApiVersion { get; set; } = "2024-02-01";
}
