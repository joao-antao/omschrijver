# What is chain-of-thought?

**Chain-of-Thought (CoT)** is a prompting technique where one asks the language model to show its reasoning process before providing the final answer. Instead of generating the output directly, the model "thinks out loud" by breaking down the problem into steps.

This technique significantly improves performance on complex tasks like math, logic, analysis, and multi-step reasoning.

# Why chain-of-thought works

Language models **do not think**, they generate tokens sequentially, and each new token can attend to all previous tokens. When we ask for reasoning first:

1. **More intermediate tokens** are generated
2. **Each token provides context** for subsequent tokens
3. **The final answer has more information** to draw from
4. **Quality improves** because the model has "seen more" relevant content

It's not human-like reasoning. This technique is **giving the model more context window to work with** before generating the final answer. Think of it like writing an essay:
- **Without CoT**: Write the conclusion immediately (hard, often shallow)
- **With CoT**: Write an outline first, then the conclusion (more coherent)

The outline provides structure and context for the conclusion.

# When to use chain-of-thought

## Use CoT when:

1. **Complex reasoning required**: Math problems, logic puzzles, multi-step analysis
2. **Transparency needed**: You want to see why the model chose a particular answer
3. **Debugging prompts**: Understanding where the model goes wrong
4. **Teaching users**: Showing the thought process adds educational value
5. **Quality matters more than cost**: CoT increases token usage but improves results

## Skip CoT When:

1. **Simple tasks**: Basic classification, short answers, straightforward rewrites
2. **Cost-sensitive**: CoT adds token overhead (reasoning tokens are charged)
3. **Speed critical**: More tokens = slower generation
4. **Reasoning not useful**: If you only care about the final answer

## In omschrijver

CoT is **optional** because:
- Most rewrites are straightforward (CoT not necessary)
- Users may want to see the reasoning for learning purposes
- Adds cost/latency for marginal quality gain in this use case

The CLI displays token usage, as example:

```
Token Usage:
  Prompt: 245 tokens
  Completion: 312 tokens (includes reasoning if enabled)
  Total: 557 tokens
```

Compare costs with/without reasoning to justify the feature.

# Research Papers

- **Original CoT Paper**: [Chain-of-Thought Prompting Elicits Reasoning in Large Language Models](https://arxiv.org/abs/2201.11903)
- **Zero-Shot CoT**: [Large Language Models are Zero-Shot Reasoners](https://arxiv.org/abs/2205.11916)
- **Self-Consistency**: [Self-Consistency Improves Chain of Thought Reasoning](https://arxiv.org/abs/2203.11171)
- **Tree of Thoughts**: [Tree of Thoughts: Deliberate Problem Solving with LLMs](https://arxiv.org/abs/2305.10601)

# Further Reading

- [IBM: Chain of Thoughts](https://www.ibm.com/think/topics/chain-of-thoughts)
- [Prompt Engineering Guide: CoT](https://www.promptingguide.ai/techniques/cot)

---

**Next**: Learn about [structured outputs](Structured-outputs) to get predictable JSON responses.