using System.Text.Json;
using System.Text.RegularExpressions;
using Mistral.SDK;
using Mistral.SDK.DTOs;
using omschrijver.cli.Models;
using omschrijver.cli.Prompts;
using omschrijver.cli.Security;

namespace omschrijver.cli.Services;

/// <summary>
/// Handles communication with the Mistral API and parsing of the response.
/// </summary>
public class RewriterService
{
    private readonly MistralClient _client;

    public RewriterService(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException(nameof(key), "MISTRAL_API_KEY environment variable is not set.");

        _client = new MistralClient(key);
    }

    /// <summary>
    /// Sends a raw property description to Mistral and returns a structured rewrite result.
    /// </summary>
    public async Task<RewriteResult> RewriteAsync(string rawDescription, bool includeReasoning)
    {
        var sanitisedDescription = InputGuard.Validate(rawDescription);

        var request = new ChatCompletionRequest(
            ModelDefinitions.MistralSmall,
            [
                new ChatMessage(ChatMessage.RoleEnum.System, PromptBuilder.SystemPrompt),
                new ChatMessage(ChatMessage.RoleEnum.User, PromptBuilder.Build(sanitisedDescription, includeReasoning))
            ],
            maxTokens: 1024, // Max tokens used for the response.
            temperature: 0.3m // Temperature controls how "random" the model's token selection is.
        );

        // Non-streaming call to get the full response at once. For larger responses, consider using streaming to process tokens as they arrive.
        var response = await _client.Completions.GetCompletionAsync(request);

        var responseText = response.Choices.First().Message.Content
                           ?? throw new InvalidOperationException("Empty response from Mistral API.");

        // Capture token usage from the API response
        var usage = new TokenUsage(
            PromptTokens: response.Usage.PromptTokens,
            CompletionTokens: response.Usage.CompletionTokens,
            TotalTokens: response.Usage.TotalTokens
        );

        return ParseResponse(responseText, usage);
    }

    /// <summary>
    /// Extracts structured JSON and optional reasoning from the model's raw response text.
    /// </summary>
    private static RewriteResult ParseResponse(string responseText, TokenUsage usage)
    {
        string? reasoning = null;
        string jsonText = responseText.Trim();

        // Extract chain-of-thought block if present
        var reasoningMatch = Regex.Match(responseText, @"<reasoning>(.*?)</reasoning>", RegexOptions.Singleline);
        if (reasoningMatch.Success)
        {
            reasoning = reasoningMatch.Groups[1].Value.Trim();
            jsonText = responseText[(reasoningMatch.Index + reasoningMatch.Length)..].Trim();
        }

        // Strip markdown code fences if the model wraps the JSON in them
        jsonText = Regex.Replace(jsonText, @"^```(json)?\s*", "", RegexOptions.Multiline)
            .TrimEnd('`')
            .Trim();

        var listing = JsonSerializer.Deserialize<PropertyListing>(jsonText)
                      ?? throw new JsonException("Deserialized listing was null.");

        return new RewriteResult(listing, reasoning, usage);
    }
}