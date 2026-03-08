# Welcome

This documentation explores some fundamental concepts behind Large Language Models (LLMs) prompt engineering, as demonstrated in the repository. While omschrijver is a CLI tool for rewriting property descriptions, this documents focuses on the **underlying concepts** that make it work. 

Each page dives into a specific concept, explaining theory, best practices, and real-world implementations.

# Concepts

## 🧩 [Tokens](Tokens.md)
Learn about the fundamental units that language models process, how tokenization works, and why token counts matter for your API usage and costs.

## 🎯 [Few-shot prompting](Few-shot-prompting.md)
Discover how providing examples to the model can dramatically improve output quality, consistency, and style without any model fine-tuning.

## 🧠 [Chain-of-thought](Chain-of-thought.md)
Explore how asking models to "think out loud" improves reasoning quality on complex tasks, and why this technique actually works.

## 📋 [Structured outputs](Structured-outputs.md)
Learn how to get predictable, parseable JSON responses from language models using schemas and proper prompt engineering.

## ⚙️ [System prompt](System-prompt.md)
Understand the instruction layer that defines your model's behavior, role, and constraints before any user interaction begins.

# About omschrijver

A (dotnet) CLI tool that transforms raw property keywords into polished, professional listing descriptions. It's built as a learning project to explore practical LLM integrations and prompt engineering techniques.

**Repository**: [github.com/joao-antao/omschrijver](https://github.com/joao-antao/omschrijver)

# Contributing

Contributions are welcome! Please open an issue or pull request in the main repository.