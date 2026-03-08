# What is few-shot prompting?

**Few-shot prompting** is a technique where one provides the language model with a small number of examples to demonstrate the desired output format, style, tone, and quality. The model learns from these examples and applies the same patterns to new inputs.

This approach requires **no model training or fine-tuning**.

# Types

## Zero-shot
No examples provided. The model relies solely on instructions:

```
Rewrite this property description professionally:
"2 bed flat, needs work"
```

## One-shot
One example provided:

```
Example:
Input: "3 bed house, garden"
Output: "Charming three-bedroom house with private garden"

Now rewrite this:
Input: "2 bed flat, needs work"
```

### Few-shot
Multiple examples (2-10) provided:

```
Example 1:
Input: "3 bed house, garden"
Output: "Charming three-bedroom house with private garden"

Example 2:
Input: "1 bed apartment, city centre"
Output: "Modern one-bedroom apartment in prime city location"

Now rewrite this:
Input: "2 bed flat, needs work"
```

# Why Few-shot works

Few-shot prompting is effective because:

1. **Pattern recognition**: Models excel at identifying and replicating patterns
2. **Implicit rules**: Examples teach subtleties that are hard to describe in words
3. **Tone & style**: Examples demonstrate the desired voice better than adjectives
4. **Format consistency**: Shows the exact output structure expected
5. **Edge case handling**: Can include examples of tricky inputs

# Few-shot in omschrijver

## Examples

omschrijver uses `Prompts/examples.json` to provide few-shot examples:

```json
[
  {
    "raw": "2 bed apartment city centre, 3rd floor, no lift, small balcony, needs updating kitchen",
    "rewritten": {
      "headline": "Characterful City-Centre Apartment with Balcony Views",
      "description": "A well-positioned two-bedroom apartment on the third floor...",
      "highlights": [
        "Two bedrooms",
        "Private balcony",
        "City-centre location",
        "Investment or renovation opportunity"
      ],
      "condition": "Modernisation required"
    }
  },
  {
    "raw": "big house 4 beds garden garage good schools nearby recently renovated move in ready",
    "rewritten": {
      "headline": "Spacious Four-Bedroom Family Home — Fully Renovated and Ready to Move In",
      "description": "An exceptional opportunity to acquire a substantially sized...",
      "highlights": [
        "Four bedrooms",
        "Fully renovated throughout",
        "Private garden and garage",
        "Highly regarded school catchment"
      ],
      "condition": "Move-in ready"
    }
  }
]
```

# Further reading

- [Language Models are Few-Shot Learners (GPT-3 Paper)](https://arxiv.org/abs/2005.14165)
- [Prompt Engineering Guide: Few-Shot Prompting](https://www.promptingguide.ai/techniques/fewshot)

---

**Next**: Learn about [chain-of-thought](Chain‐of‐thought) prompting for complex reasoning tasks.