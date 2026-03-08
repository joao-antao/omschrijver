# What are structured outputs?

**Structured outputs** are responses that follow a predefined format, typically JSON, making them predictable, parseable, and safe to integrate into applications. Instead of free-form text, the model returns data that conforms to a schema.

This is essential for building reliable applications that consume LLM responses programmatically.

# Issues with free-form text

Without structure, LLM outputs are unpredictable:

```
User: "Rewrite this property description: 2 bed flat, garden"

Model Response (could be any of these):
- "A lovely two-bedroom flat with garden"
- "**Flat**: 2 bedrooms\n**Garden**: Yes"
- "{ 'beds': 2, 'has_garden': true }"
- "This property features two bedrooms and a private garden area..."
```

**Problems**:
- Can't reliably extract specific fields
- Format changes between requests
- Difficult to validate
- Parsing breaks easily

# Solution: define a schema

Describe to the model exactly what structure you want:

```json
{
  "headline": "string (max 12 words)",
  "description": "string (3-4 sentences)",
  "highlights": ["string", "string", ...],
  "condition": "string (one short phrase)"
}
```

Now the model knows:
- What fields to include
- What type each field should be
- Any constraints (length, format, etc.)

# Structured outputs in omschrijver

## Data model

I've defined the expected structure as a C# record:

```csharp
public record PropertyListing(
    [property: JsonPropertyName("headline")]    string Headline,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("highlights")]  List<string> Highlights,
    [property: JsonPropertyName("condition")]   string Condition
);
```

This serves as:
1. **Documentation**: developers know what fields exist
2. **Type Safety**: compiler catches errors
3. **Schema**: can be converted to JSON Schema for the prompt

## Parsing the response

Deserialize JSON into the strongly-typed model:

```csharp
private static RewriteResult ParseResponse(string responseText, TokenUsage usage)
{
    string jsonText = responseText.Trim();

    // Extract reasoning if present
    var reasoningMatch = Regex.Match(responseText, @"<reasoning>(.*?)</reasoning>", RegexOptions.Singleline);
    if (reasoningMatch.Success)
    {
        jsonText = responseText[(reasoningMatch.Index + reasoningMatch.Length)..].Trim();
    }

    // Strip markdown code fences if present
    jsonText = Regex.Replace(jsonText, @"^```(json)?\s*", "", RegexOptions.Multiline)
                    .TrimEnd('`')
                    .Trim();

    // Deserialize to strongly-typed model
    var listing = JsonSerializer.Deserialize<PropertyListing>(jsonText)
        ?? throw new JsonException("Deserialized listing was null.");

    return new RewriteResult(listing, reasoning, usage);
}
```

# Further Reading

- [OpenAI: JSON Mode](https://platform.openai.com/docs/guides/structured-outputs)
- [Guidance (Structured Generation)](https://github.com/guidance-ai/guidance)

---

**Next**: Learn about [system prompt](System-prompt) to define your model's behavior and constraints.