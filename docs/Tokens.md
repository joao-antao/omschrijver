# What is a token?

A token is the smallest unit of text that a model, such as a LLM processes. It can be a word, part of a word, punctuation, or even a space, depending on how the text is split.
As example, the sentence "Tokenization is important for AI." might be tokenized (typically) as:

```
["Token", "ization", " is", " important", " for", " AI", "."]
```

The total number tokens in this example is 7. The model processes these tokens, not the raw text. The process of converting text into tokens is called **tokenization**.

If you are familiar with search engines, such as lucene, you might understand the core idea. With search engines tokens are used to break down text into searchable terms for indexing and retrieval. On LLMs, tokens are the building blocks of how models understand and generate language, convert text into numerical inputs for the neural networks and understand context better.

# Why tokens matter

## 1. **Costs**

Most LLM APIs charge per token:
- Input tokens: The prompt you send
- Output tokens: The response generated
- Total cost = (input_tokens × input_price) + (output_tokens × output_price)

In omschrijver, token usage is tracked and displayed after each request:

```csharp
var usage = new TokenUsage(
    PromptTokens: response.Usage.PromptTokens,
    CompletionTokens: response.Usage.CompletionTokens,
    TotalTokens: response.Usage.TotalTokens
);
```

## 2. **Context limits**

Every model has a maximum context window (e.g., 8K, 32K, 128K tokens). This limit includes:
- System prompt
- Few-shot examples
- User input
- Model output

Exceeding this limit causes errors or truncation.

## 3. **Response speed**

More tokens = longer generation time. The model generates tokens sequentially, so:
- A 100-token response is ~2× faster than 200 tokens
- Streaming helps show progress for long responses

## 4. **Quality trade-offs**

Token limits force decisions:
- More examples = better quality but fewer tokens for the actual task
- Longer system prompts = clearer instructions but higher base cost
- Detailed output = better results but more expensive

# Token optimization

## Setting max tokens

```csharp
var request = new ChatCompletionRequest(
    ModelDefinitions.MistralSmall,
    messages,
    maxTokens: 1024, // Limit response size
    temperature: 0.3m
);
```

The `maxTokens` parameter:
- Caps the maximum response length
- Prevents runaway generation
- Controls costs
- Should be set based on expected output size

## Choosing the model

Omschrijver uses `MistralSmall`:
- **Smaller models**: Faster, cheaper, shorter context windows
- **Larger models**: More capable, slower, more expensive, longer context

# Tokenization algorithms

* **Byte Pair Encoding (BPE)**: The most common method, used by models like GPT. It starts with individual characters and iteratively merges the most frequent pairs into single tokens.
* **WordPiece:** Similar to BPE, used by BERT and other models. It balances word and subword tokens.
* **SentencePiece:** A language-agnostic tokenizer that works even for languages without spaces (e.g., Chinese, Japanese).

Each algorithm affects how words, names, and technical terms are split, which impacts model performance.

# Token tooling

- **OpenAI Tokenizer**: [platform.openai.com/tokenizer](https://platform.openai.com/tokenizer)
- **tiktoken** (Python): `pip install tiktoken` (also developed by OpenAI)

# Further reading

- [Mistral AI documentation](https://docs.mistral.ai/)
- [Understanding tokenization in NLP](https://huggingface.co/docs/transformers/tokenizer_summary)
- [Token Pricing comparison](https://openai.com/pricing)

---

**Next**: Learn about [few-shot prompting](Few‐shot-prompting) to improve output quality with examples.