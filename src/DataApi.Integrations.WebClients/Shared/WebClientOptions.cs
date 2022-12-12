namespace DataApi.Integrations.WebClients.Shared;

public abstract class WebClientOptions
{
    public string BaseAddress { get; set; } = null!;
    public int? TimeoutInMilliseconds { get; set; }
    public string DependencyName { get; set; } = null!;
}