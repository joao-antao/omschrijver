using omschrijver.cli.Configuration;

namespace omschrijver.cli.Display;

/// <summary>
/// Parses command-line arguments into a <see cref="CliOptions"/> instance.
/// Returns null if the user requested help (process should exit cleanly).
/// </summary>
public static class ArgumentParser
{
    public static CliOptions? Parse(string[] args)
    {
        string? input      = null;
        string? filePath   = null;
        bool includeReasoning = true; // Default is to include reasoning

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--input" or "-i" when i + 1 < args.Length:
                    input = args[++i];
                    break;

                case "--file" or "-f" when i + 1 < args.Length:
                    filePath = args[++i];
                    break;

                case "--no-reasoning":
                    includeReasoning = false;
                    break;

                case "--help" or "-h":
                    PrintHelp();
                    return null;

                default:
                    Console.Error.WriteLine($"Unknown argument: {args[i]}");
                    PrintHelp();
                    return null;
            }
        }

        return new CliOptions(input, filePath, includeReasoning);
    }

    private static void PrintHelp()
    {
        Console.WriteLine("""
            Omschrijver (property description rewriter)

            Usage:
              dotnet run
              dotnet run -- --input "3 bed flat, sea views, parking"
              dotnet run -- --file my_listing.txt
              dotnet run -- --input "..." --no-reasoning

            Options:
              --input,  -i   Raw description as a string
              --file,   -f   Path to a text file with the raw description
              --no-reasoning Skip chain-of-thought reasoning (compare quality difference)
              --help,   -h   Show this help message
            """);
    }
}
