using LangChain.Chains.LLM;
using LangChain.Prompts;

namespace Backend.Services;

public class LlmChainProvider(IEmbeddingProvider embeddingProvider) : ILlmChainProvider
{
    public LlmChain GetMode()
    {
        var input = new PromptTemplateInput("", new List<string>());
        var prompt = new PromptTemplate(input);
        var llmChainInput = new LlmChainInput(embeddingProvider.GetModel(), prompt);
        var llmChain = new LlmChain(llmChainInput);
        return llmChain;
    }
}