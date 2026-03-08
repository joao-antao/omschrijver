/*
 * Omschrijver
 * ==============
 *
 * Rewrites raw property descriptions into polished and professional listings.
 *
 * Usage:
 *   dotnet run
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using omschrijver.cli.Configuration;
using omschrijver.cli.Display;
using omschrijver.cli.Services;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<MistralOptions>()
    .Build();

var services = new ServiceCollection()
    .Configure<MistralOptions>(configuration.GetSection(nameof(MistralOptions)))
    .AddOptions()
    .BuildServiceProvider();

var mistralOptions = services.GetRequiredService<IOptions<MistralOptions>>();

InteractivePrompt.ShowWelcome();

var service = new RewriterService(mistralOptions.Value.ApiKey);

while (true)
{
    try
    {
        var options = InteractivePrompt.GetUserOptions();
        
        var rawDescription = await InputResolver.ResolveAsync(options);
        if (string.IsNullOrWhiteSpace(rawDescription))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: no input provided.");
            Console.ResetColor();
            continue;
        }

        ConsoleDisplay.PrintProgress("Rewriting with Mistral (Le Chat)");
        var result = await service.RewriteAsync(rawDescription, includeReasoning: options.IncludeReasoning);
        ConsoleDisplay.PrintDone();

        ConsoleDisplay.PrintResult(result, rawDescription);
        
        if (!InteractivePrompt.AskToContinue())
            break;
            
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nError: {ex.Message}");
        Console.ResetColor();
        
        if (!InteractivePrompt.AskToContinue())
            break;
    }
}

Console.WriteLine("\nThank you for using Omschrijver! Goodbye! 👋");
Console.WriteLine();
