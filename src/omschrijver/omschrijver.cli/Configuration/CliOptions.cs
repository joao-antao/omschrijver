namespace omschrijver.cli.Configuration;

/// <summary>
/// Parsed CLI options.
/// </summary>
public record CliOptions(
    string? Input,
    string? FilePath,
    bool IncludeReasoning
);