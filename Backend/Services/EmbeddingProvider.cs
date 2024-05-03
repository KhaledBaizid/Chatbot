using LangChain.Providers.OpenAI;

namespace Backend.Services;

public class EmbeddingProvide(string apiKey) : IEmbeddingProvider
{
    public Gpt35Turbo16KModel GetModel()
    {
        return new Gpt35Turbo16KModel(apiKey);

    }
    
    
}