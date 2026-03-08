# Omschrijver

A CLI tool that rewrites raw unformatted property descriptions into polished, professional listings using Mistral (API).

## Features

- 🤖 **AI-powered rewriting**: Uses Mistral to transform property keywords into professional listing descriptions
- 💭 **Chain-of-thought**: Optional reasoning output to see how the model analyzes the input
- 📝 **Structured output**: Consistently generates headline, description, highlights, and condition assessment
- 🔄 **Interactive interface**: User-friendly prompts guide you through the process
- 📁 **File Support**: Load descriptions from text files or type them directly


## Concepts explored 

* **Tokens**: Tokens serve as the smallest individual units that a language model processes, typically representing common sequences of characters such as words or subwords
* **System prompt**: The instruction layer that runs before any user interaction, setting the model's role, rules, constraints, and behaviour
* **Few-shot prompting**: Giving the model examples to shape tone, format, and length
* **Chain-of-thought**: Scaffolding intermediate output to improve quality on complex tasks, and importantly, why it works (more tokens = more context for the final output, no human reasoning)
* **Structured outputs**: Using a JSON schema to make responses predictable and parseable


## Usage

Simply run the application:

```bash
dotnet run
```

## Building

```bash
dotnet build
```

## Requirements

- .NET 10.0 SDK
- Mistral API key (configured via user secrets or environment variables)

## Project structure

```
omschrijver.cli/
├── Program.cs                      # Main entry point with interactive loop
├── Configuration/
│   ├── CliOptions.cs               # CLI configuration options
│   └── MistralOptions.cs           # Mistral configuration options
├── Display/
│   ├── ArgumentParser.cs           # Argument parsing (not used)
│   ├── ConsoleDisplay.cs           # Formatted output display
│   ├── InputResolver.cs            # Resolves input from various sources
│   └── InteractivePrompt.cs        # Interactive user input prompts
├── Models/
│   └── PropertyListing.cs          # Data models
├── Prompts/
│   ├── examples.json               # Few-shot examples for the model
│   ├── PromptBuilder.cs            # Prompt construction
│   ├── reasoning-instruction.md    # Chain-of-thought instructions
│   └── system-prompt.md            # System prompt template
├── Security/
│   └── InputGuard.cs               # Sanitise and validate input
└── Services/
    └── RewriterService.cs          # Mistral API integration
```

## Disclaimer

This is a learning project to explore building a CLI tool while learning prompt engineering fundamentals.
