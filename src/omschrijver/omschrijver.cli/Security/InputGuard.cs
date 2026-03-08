namespace omschrijver.cli.Security;

/// <summary>
/// Guards against prompt injection by sanitising and validating raw user input before it is inserted into a prompt.
/// Defence 1: Input length limit (rejects suspiciously long inputs)
/// Defence 2: Input sanitisation (strips characters used in injection attacks)
/// </summary>
public static class InputGuard
{
    private const int MaxInputLength = 500;

    private static readonly char[] SuspiciousCharacters = ['<', '>', '{', '}', '`'];

    /// <summary>
    /// Validates and sanitises a raw property description.
    /// Throws <see cref="InvalidOperationException"/> if the input is rejected.
    /// Returns the sanitised input if it passes all checks.
    /// </summary>
    public static string Validate(string rawInput)
    {
        if (rawInput.Length > MaxInputLength)
            throw new InvalidOperationException(
                $"Input too long ({rawInput.Length} chars). Maximum is {MaxInputLength} characters. " +
                "Please provide a concise property description.");

        var sanitised = rawInput;
        foreach (var ch in SuspiciousCharacters)
            sanitised = sanitised.Replace(ch.ToString(), string.Empty);

        var injectionPatterns = new[]
        {
            "ignore previous instructions",
            "ignore all instructions",
            "disregard your instructions",
            "you are now",
            "forget your instructions",
            "system prompt",
            "pretend you are",
        };

        var lowerInput = sanitised.ToLowerInvariant();
        foreach (var pattern in injectionPatterns)
        {
            if (lowerInput.Contains(pattern))
                throw new InvalidOperationException(
                    $"Input contains disallowed content: \"{pattern}\". " +
                    "Please provide a genuine property description.");
        }

        return sanitised.Trim();
    }
}