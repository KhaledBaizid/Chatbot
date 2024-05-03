using LangChain.Chains.LLM;

namespace Backend.Services;

public interface IPromptProvider
{
    public LlmChain GetModeLlmChain();
}