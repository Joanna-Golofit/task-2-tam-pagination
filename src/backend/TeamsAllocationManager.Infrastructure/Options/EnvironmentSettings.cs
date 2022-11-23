namespace TeamsAllocationManager.Infrastructure.Options;

public class EnvironmentSettings
{
	public string TestMailAddress { get; set; } = null!;
	public EnvironmentType EnvironmentType { get; set; } = EnvironmentType.Stage;
	public string AppUrl { get; set; } = null!;
}

public enum EnvironmentType
{
	Stage,
	Production
}
