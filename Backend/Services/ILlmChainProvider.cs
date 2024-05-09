using LangChain.Chains.LLM;

namespace Backend.Services;

public interface ILlmChainProvider
{
    public LlmChain GetMode();
}