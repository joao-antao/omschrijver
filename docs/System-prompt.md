# What is a system prompt?

A **system prompt** (also called "system message" or "instruction layer") is a special message that sets the model role, behavior, rules, and constraints *before* any user interaction begins. It defines "who" the model is and "how" it should respond.

The model treats system prompts as higher-trust than user input. There are four components of a well-designed system prompt.

## 1. Role definition

Tell the model who it is. This anchors its behaviour across the whole conversation.

```
// Weak (because too vague):
You are a helpful assistant.

// Strong (because assumes a specific role with context)
You are a professional real estate copywriter specialising in the Dutch residential property market.
The more specific the role, the more consistently the model behaves. "Dutch residential property market" is better than just "real estate" because it subtly steers the model toward appropriate terminology, price ranges, and conventions.
```

## 2. Task definition

Tell the model what it does. One clear sentence is better than a paragraph.

```
// Weak (because describes a problem)
You help people write better property descriptions because raw 
descriptions from agents are often messy and inconsistent.

// Strong (because describes the action)
Rewrite raw, unformatted property descriptions into polished, 
professional listings.
```

## 3. Constraints and rules

Tell the model what it must and must not do. Be explicit, the model won't infer constraints you don't state.

```
// Weak (leaves room for interpretation)
- Write professionally
- Include key details

// Strong (precise and enforceable)
- Tone: confident, factual, never hyperbolic
- Headline: max 12 words
- Never invent features not mentioned in the raw input
- Always respond with valid JSON
```

The "never invent features" rule is particularly important for application, without it, the model might helpfully add "south-facing garden" to a listing that never mentioned it. That's a hallucination with real-world consequences.

## 4. Output format

Tell the model exactly what shape the response should be. We are doing this with the JSON schema, but it belongs in the system prompt, not just the user prompt, to reinforce it.


## Ordering matters

Models pay more attention to content at the beginning and end of the context window, the middle gets less weight. This is sometimes called the **"lost in the middle"** problem.
For system prompts the practical rule is:

* Put the role and task first — the model reads this before anything else
* Put the most critical constraints at the top of the rules list
* Put the output format last — it's the last thing the model reads before generating

In omschrijver the most critical rule is "Never invent or imply features not explicitly mentioned in the raw input". It should be at the top of the rules list, not buried in the middle.

# Positive vs negative instructions

Models respond better to positive instructions than negative ones. Tell the model what to do, not just what not to do.

```
// Negative (less effective)
- Don't be vague
- Don't use hyperbole
- Don't invent features

// Positive (more effective)
- Be specific and factual
- Use measured, professional language
- Only describe features explicitly mentioned in the raw input
```

A mix works well — lead with positive instructions, use negative ones sparingly for hard constraints.

# System prompt in omschrijver

## The Prompt

```markdown
You are a professional real estate copywriter specialising in the
Dutch residential property market. Rewrite raw, unformatted property
descriptions into polished, professional listings.

Rules:
- Never invent or imply features not explicitly mentioned in the raw input
- Only describe features explicitly mentioned in the raw input
- Tone: confident, factual, never hyperbolic
- Headline: concise and compelling, max 12 words
- Description: 3–4 sentences, no bullet points
- Highlights: 3–5 short bullet points
- Condition: one short phrase
- Always respond with valid JSON matching the exact schema provided
```

## What this achieves

| Element | Purpose |
|---------|---------|
| **"You are a professional real estate copywriter"** | Sets role & expertise level |
| **"specialising in the Dutch residential property market"** | Narrow the domain |
| **"Never invent or imply features"** | Critical constraint (prevents hallucination) |
| **"Tone: confident, factual, never hyperbolic"** | Defines writing style |
| **"max 12 words"**, **"3–4 sentences"** | Specific format constraints |
| **"Always respond with valid JSON"** | Output format requirement |

## How it's used

```csharp
var request = new ChatCompletionRequest(
    ModelDefinitions.MistralSmall,
    messages: [
        new ChatMessage(
            ChatMessage.RoleEnum.System, 
            PromptBuilder.SystemPrompt  // Loaded from Prompts/system-prompt.md
        ),
        new ChatMessage(
            ChatMessage.RoleEnum.User, 
            PromptBuilder.Build(rawDescription, includeReasoning)
        )
    ],
    maxTokens: 1024,
    temperature: 0.3m
);
```

The system prompt runs **once per request** and applies to **every user input**.

## When to include information in System vs. User Prompts

**System Prompt**:
- Identity and role
- Universal rules and constraints
- Output format requirements
- Tone and style guidelines

**User Message**:
- The specific task
- Input data
- Few-shot examples
- Chain-of-thought instructions (if optional)
- Context that changes per request

## Example

**System**:
```
You are a real estate copywriter. Never invent features. 
Always respond with valid JSON. Tone: professional, factual.
```

**User**:
```
Examples:
[few-shot examples here]

Now rewrite this property:
"2 bed flat garden"
```

# Temperature influence

System prompts interact with API parameters, such as temperature.

Controls randomness (0.0 = deterministic, 1.0 = creative):

```csharp
temperature: 0.3m  // omschrijver uses low temperature
```

- **Low (0.0-0.3)**: Good for structured outputs, factual tasks, consistency
- **Medium (0.4-0.7)**: Balanced creativity and coherence
- **High (0.8-1.0)**: Maximum creativity, useful for brainstorming

Omschrijver uses **0.3** because:
- Property descriptions need consistency
- Factual accuracy is critical
- Creative variation is less important

# Common mistakes

* **Too generic**: "You are a helpful assistant" (teaches nothing)
* **Too long**: 1000-word system prompts waste tokens and confuse models
* **No constraints**: Fails to prevent common errors
* **Mixing concerns**: Put task-specific info in user messages, not system
* **Never updating**: System prompts should evolve based on real results
* **No examples**: Sometimes examples in system prompt help (though few-shot is better)
* **Weak injection defense**: User input can override instructions

# Further reading

- [Anthropic: System Prompts (Claude)](https://docs.anthropic.com/claude/docs/system-prompts)
- [Prompt Injection: What's the Worst That Can Happen?](https://simonwillison.net/2023/Apr/14/worst-that-can-happen/)
- [OWASP Top 10 for LLM Applications](https://owasp.org/www-project-top-10-for-large-language-model-applications/)