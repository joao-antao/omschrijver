using System.Text.Json.Serialization;

namespace omschrijver.cli.Models;

/// <summary>
/// Represents a rewritten property listing returned by Claude.
/// </summary>
public record PropertyListing(
    [property: JsonPropertyName("headline")]    string Headline,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("highlights")]  List<string> Highlights,
    [property: JsonPropertyName("condition")]   string Condition
);

/// <summary>
/// Token usage statistics returned by the Mistral API.
/// </summary>
public record TokenUsage(int PromptTokens, int CompletionTokens, int TotalTokens);

/// <summary>
/// Wraps the parsed listing together with optional chain-of-thought reasoning
/// and token usage statistics.
/// </summary>
public record RewriteResult(PropertyListing Listing, string? Reasoning, TokenUsage Usage);
