namespace TeamsAllocationManager.Domain.Enums;

// Attention: Be carefull when managing names of enum items. They are used in database (string conversion. max length 30).
public enum DbConfigKey
{
	IgnoredProjects,
	Divisions
}
