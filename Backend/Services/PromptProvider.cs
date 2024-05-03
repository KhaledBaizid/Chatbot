using LangChain.Chains.LLM;
using LangChain.Prompts;

namespace Backend.Services;

public class PromptProvider(IEmbeddingProvider gptService) : IPromptProvider
{
    public LlmChain GetModeLlmChain()
    {
        var input = new PromptTemplateInput("", new List<string>());
        var prompt = new PromptTemplate(input);
        var llmChainInput = new LlmChainInput(gptService.GetModel(), prompt);
        var llmChain = new LlmChain(llmChainInput);
        return llmChain;
    }
}