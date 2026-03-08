using System.Text;
using System.Text.Json;
using omschrijver.cli.Models;

namespace omschrijver.cli.Prompts;

/// <summary>
/// Builds the prompts sent to Mistral, combining:
///   - A system prompt defining the model's role and rules
///   - Few-shot examples teaching the desired output format
///   - An optional chain-of-thought reasoning instruction
///   - A structured output schema
/// </summary>
public static class PromptBuilder
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };
    private static readonly string PromptsFolder = Path.Combine(AppContext.BaseDirectory, "Prompts");

    private static readonly Lazy<string> Prompt = new(() => 
        File.ReadAllText(Path.Combine(PromptsFolder, "system-prompt.md")).Trim());
    
    private static readonly Lazy<ExamplePair[]> Examples = new(() =>
    {
        var json = File.ReadAllText(Path.Combine(PromptsFolder, "examples.json"));
        return JsonSerializer.Deserialize<ExamplePair[]>(json) 
            ?? throw new InvalidOperationException("Failed to load examples.json");
    });

    // When applying chain-of-thought we ask the model to generate intermediate text before producing the final output.
    // The intermediate text ("reasoning"), tends to improve output quality, and there are a few explanations for why:
    // The more grounded explanation is that it's essentially buying the model more tokens to work with.
    // LLMs predict one token at a time, and each token is influenced by everything before it.
    // By forcing the model to write out intermediate steps, you're giving it more context to condition the final answer on.
    // The output is better not because the model "thought harder" (they don't think!) but because the generated reasoning text is now part of the input for the final JSON.
    private static readonly Lazy<string> ReasoningInstruction = new(() =>
        File.ReadAllText(Path.Combine(PromptsFolder, "reasoning-instruction.md")).Trim());

    /// <summary>
    /// The system prompt loaded from system-prompt.md
    /// </summary>
    public static string SystemPrompt => Prompt.Value;

    /// <summary>
    /// Builds the full user prompt for a given raw description.
    /// </summary>
    /// <param name="rawDescription">The unformatted listing text.</param>
    /// <param name="includeReasoning">
    ///   When true, asks the model to reason step-by-step before producing JSON.
    ///   This is chain-of-thought prompting, it improves quality on tasks
    ///   requiring judgement, at the cost of a slightly longer response.
    /// </param>
    public static string Build(string rawDescription, bool includeReasoning)
    {
        var examplesBlock = BuildExamplesBlock();
        var reasoningBlock = includeReasoning ? $"\n\n{ReasoningInstruction.Value}" : string.Empty;
        var schema = BuildOutputSchema();

        return $"""
            Here are two examples of the transformation you should perform:
            {examplesBlock}
            {reasoningBlock}
            Now rewrite the following raw property description.
            Respond with JSON matching this exact schema:
            {schema}

            Raw input: {rawDescription}
            """;
    }

    private static string BuildExamplesBlock()
    {
        var builder = new StringBuilder();
        var examples = Examples.Value;
        
        for (int i = 0; i < examples.Length; i++)
        {
            var example = examples[i];
            builder.AppendLine($"\nExample {i + 1}:");
            builder.AppendLine($"Raw input: {example.Raw}");
            builder.AppendLine($"Output: {JsonSerializer.Serialize(example.Rewritten, JsonOptions)}");
        }
        
        return builder.ToString();
    }

    private static string BuildOutputSchema()
    {
        return JsonSerializer.Serialize(new
        {
            headline    = "string — compelling title, max 12 words",
            description = "string — 3 to 4 sentences of professional prose",
            highlights  = new[] { "string", "string", "...up to 5 items" },
            condition   = "string — one short phrase"
        }, JsonOptions);
    }

    private record ExamplePair(string Raw, PropertyListing Rewritten);
}
