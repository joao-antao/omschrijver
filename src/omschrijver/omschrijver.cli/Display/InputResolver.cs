using omschrijver.cli.Configuration;

namespace omschrijver.cli.Display;

/// <summary>
/// Resolves the raw property description from one of three sources:
///   1. --input argument (inline string)
///   2. --file argument (text file)
///   3. Interactive prompt (user types directly in the terminal)
/// </summary>
public static class InputResolver
{
    public static async Task<string?> ResolveAsync(CliOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.Input))
            return options.Input.Trim();

        if (!string.IsNullOrWhiteSpace(options.FilePath))
            return (await File.ReadAllTextAsync(options.FilePath)).Trim();

        return ReadInteractive();
    }

    private static string? ReadInteractive()
    {
        Console.WriteLine("\nEnter raw property description (press Enter twice when done):");

        var lines = new List<string>();
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) break;
            lines.Add(line);
        }

        var result = string.Join(" ", lines).Trim();
        return string.IsNullOrWhiteSpace(result) ? null : result;
    }
}
